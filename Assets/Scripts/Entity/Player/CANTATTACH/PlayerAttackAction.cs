using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TM.Entity.Player
{
    public class PlayerAttackAction
    {
        public enum AttackPattern
        {
            NormalAttack,
            FallAttack
        }

        //どのくらいの角度をサーチするか。[単位:度数法]
        //この値だけでなく、UIの変更も必要。
        const float SEARCH_ANGLE = 30f;
        
        //団子までたどり着く時間
        const float RUSH_TIME = 0.2f;

        Animator _animator;
        SpitManager _spitManager;

        List<DangoData> _targetDangoList = new();
        DangoData _highlightDango;

        bool _hasStabDango = false;

        public PlayerAttackAction(Animator animator, SpitManager spitManager)
        {
            _animator = animator;
            _spitManager = spitManager;
        }

        public void Update(Transform transform)
        {
            if (_targetDangoList.Count == 0) return;

            float min = float.MaxValue;
            int index = -1;

            //最も近くの団子をターゲットする
            for (int i = 0; i < _targetDangoList.Count; i++)
            {
                Vector3 vec = _targetDangoList[i].transform.position - transform.position;

                //レイを飛ばして、間に壁があるか判定
                if (Physics.Raycast(transform.position, vec, out RaycastHit hit, vec.magnitude, ~1 << LayerMask.NameToLayer("MapCollider")))
                {
                    if (hit.collider.GetComponent<DangoData>() == null) continue;
                }
                float targetAngle = Vector3.Angle(transform.forward, vec);

                //サーチする角度より大きかったらやめる
                if (targetAngle >= SEARCH_ANGLE) continue;

                float distance = Vector3.Distance(transform.position, _targetDangoList[i].transform.position);

                min = Mathf.Min(min, distance);
                index = i;
            }

            if (index == -1)
            {
                if (_highlightDango != null)
                {
                    _highlightDango.gameObject.SetLayerIncludeChildren(0);
                    _highlightDango = null;
                }
                return;
            }

            //ハイライト表示
            if (_highlightDango != _targetDangoList[index])
            {
                if (_highlightDango != null) _highlightDango.gameObject.SetLayerIncludeChildren(0);
                _highlightDango = _targetDangoList[index];
                _highlightDango.gameObject.SetLayerIncludeChildren(9);
            }
        }

        public bool ChangeState(AttackPattern attack)
        {
            return attack == AttackPattern.NormalAttack ? IsWaitingNormalAttack() : IsWaitingFallAttackAnimation();
        }

        //落下アクション中の突き刺し
        private bool IsWaitingFallAttackAnimation()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("AN1")) return true;
            if (!stateInfo.IsName("AN7B")) return false;

            _spitManager.IsSticking = false;
            return stateInfo.normalizedTime >= 0.7f;
        }

        //通常の突き刺し
        private bool IsWaitingNormalAttack()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if (!stateInfo.IsName("AN5")) return false;

            return stateInfo.normalizedTime >= 0.9f;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out DangoData dango))
            {
                _targetDangoList.Add(dango);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out DangoData dango))
            {
                _targetDangoList.Remove(dango);
            }
        }

        public async void WasPressedAttackButton(Rigidbody rb)
        {
            if (_highlightDango is null) return;
            _hasStabDango = false;

            float distance = Vector3.Distance(_highlightDango.transform.position, rb.transform.position);

            //そっちに向いて
            rb.transform.LookAt(_highlightDango.transform.position.SetY(rb.position.y));

            //前進(距離/時間を第一引数とすれば良い)
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
            {
                await UniTask.WaitForFixedUpdate();

                //突進中に刺さったら急停止
                if (_hasStabDango)
                {
                    _hasStabDango = false;
                    rb.velocity = Vector3.zero;
                    break;
                }

                //上への運動を断ち切りたいため直接書き換え
                rb.velocity = (rb.transform.forward.normalized * distance / RUSH_TIME).SetY(0);
            }

            //ターゲット一覧から今の団子を消す。(乱入されて別の団子が刺さったときにこの団子が再度ターゲットされにくい)
            _targetDangoList.Remove(_highlightDango);
        }

        public void SetHasStabDango()
        {
            _hasStabDango = true;
        }
    }
}
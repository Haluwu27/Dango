using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        const float ANIMATION_FLAME = 11f / 30f;

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

        public void FixedUpdate(Transform transform)
        {
            if (_targetDangoList.Count == 0)
            {
                _highlightDango = null;
                return;
            }

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

                Debug.DrawRay(transform.position, vec, Color.blue);

                //サーチする角度より大きかったらやめる
                if (targetAngle >= SEARCH_ANGLE) continue;

                float distance = Vector3.Distance(transform.position, _targetDangoList[i].transform.position);

                if (distance < min)
                {
                    min = distance;
                    index = i;
                }
            }

            if (index == -1)
            {
                ResetHighlightDango();
                return;
            }

            //ハイライト表示
            SetHighlightDango(_targetDangoList[index]);
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

        public void WasPressedAttackButton(Rigidbody rb)
        {
            if (_highlightDango is null)
            {
                //ノーターゲットでも前進する
                RushStart(rb, 2f);
                return;
            }

            //ハイライトした団子だけに刺さるようにする
            _highlightDango.SetIsMoveable(false);
            _highlightDango.Rb.velocity = Vector3.zero;

            _hasStabDango = false;

            float distance = Vector3.Distance(_highlightDango.transform.position, rb.transform.position);

            //そっちに向いて
            rb.transform.LookAt(_highlightDango.transform.position.SetY(rb.position.y));

            //前進(距離を第二引数とすれば良い)
            RushStart(rb, distance);
        }

        private async void RushStart(Rigidbody rb, float distance)
        {
            //別アニメーションが終わりきっていなければ終了まで待機
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > ANIMATION_FLAME)
            {
                while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime > ANIMATION_FLAME)
                {
                    await UniTask.Yield();
                }
            }

            //念のためのバグチェック
            Logger.Assert(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= ANIMATION_FLAME);

            //前進するフレームまで待機
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= ANIMATION_FLAME)
            {
                await UniTask.Yield();

            }

            //突き刺せる状態にして
            _spitManager.IsSticking = true;

            //上への運動を断ち切りたいため直接書き換え
            rb.velocity = (rb.transform.forward.normalized * distance / RUSH_TIME).SetY(0);

            //ここも突進中刺さったら停止を常にやる
            OnRush(rb);
        }

        private async void OnRush(Rigidbody rigidbody)
        {
            try
            {
                while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
                {
                    await UniTask.Yield();

                    //突進中に刺さったら急停止
                    if (_hasStabDango)
                    {
                        _hasStabDango = false;
                        rigidbody.velocity = Vector3.zero;

                        //ターゲット一覧から今の団子を消す。
                        ResetHighlightDango();
                        return;
                    }
                }
            }
            catch (MissingReferenceException)
            {
                return;
            }
        }

        public void SetHasStabDango()
        {
            _hasStabDango = true;
        }

        private void SetHighlightDango(DangoData dango)
        {
            //現在ハイライト中の団子と一致していたら弾く
            if (_highlightDango == dango) return;

            //ハイライト中の団子が既に存在していたらハイライトをやめる
            ResetHighlightDango();

            //セット
            _highlightDango = dango;
            _highlightDango.gameObject.SetLayerIncludeChildren(9);
        }

        private void ResetHighlightDango()
        {
            if (_highlightDango == null) return;

            _highlightDango.gameObject.SetLayerIncludeChildren(0);
            _highlightDango = null;
        }

        public void RemoveTargetDango(DangoData dango)
        {
            bool succees = _targetDangoList.Remove(dango);
            Logger.Assert(succees);
        }

        public void SetSpitManager(SpitManager spitManager) => _spitManager = spitManager;
    }
}
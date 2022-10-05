using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerRemoveDango
    {
        List<DangoColor> _dangos;
        DangoUIScript _dangoUIScript;
        PlayerData _playerData;
        Animator _animator;

        const float ANIMATION_TIME = 1f;
        
        //浮遊力
        const float FLOATING_POWER = 3f;

        bool _isPressFire;
        bool _isCoolDown;

        public bool IsCoolDown => _isCoolDown;

        public PlayerRemoveDango(List<DangoColor> dangos, DangoUIScript dangoUIScript, PlayerData playerData, Animator animator)
        {
            _dangos = dangos;
            _dangoUIScript = dangoUIScript;
            _playerData = playerData;
            _animator = animator;
        }

        //団子弾(取り外し)
        public async void Remove()
        {
            //串に何もなかったら実行しない。
            if (_dangos.Count == 0) return;

            await CoolTime();

            _isCoolDown = false;

            if (!_isPressFire) return;

            //AN8B再生
            _animator.CrossFade("DangoRemoving", 0f);

            //[Debug]何が消えたかわかるやつ
            //今までは、dangos[dangos.Count - 1]としなければなりませんでしたが、
            //C#8.0以降では以下のように省略できるようです。
            //問題は、これを知らない人が読むとわけが分からない。
            Logger.Log(_dangos[^1]);

            //SE
            SoundManager.Instance.PlaySE(SoundSource.SE9_REMOVE_DANGO);

            //消す処理。
            _dangos.RemoveAt(_dangos.Count - 1);

            //UI更新
            _dangoUIScript.DangoUISet(_dangos);
        }

        private async UniTask CoolTime()
        {
            _isPressFire = InputSystemManager.Instance.IsPressFire;

            if (!_playerData.IsGround)
            {
                //空中時に行う処理
                _playerData.Rb.velocity = _playerData.Rb.velocity.SetY(FLOATING_POWER);

                return;
            }

            _isCoolDown = true;

            //AN8A再生
            _animator.SetTrigger("IsPressFireTrigger");

            float time = ANIMATION_TIME;

            while (time > 0)
            {
                if (!InputSystemManager.Instance.IsPressFire)
                {
                    _isPressFire = InputSystemManager.Instance.IsPressFire;
                    _animator.SetTrigger("IsReleaseFireTrigger");
                    return;
                }

                await UniTask.Yield();

                time -= Time.deltaTime;
            }
        }
    }
}
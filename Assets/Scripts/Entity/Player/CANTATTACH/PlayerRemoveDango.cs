using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerRemoveDango
    {
        List<DangoColor> _dangos;
        DangoUIScript _dangoUIScript;

        public PlayerRemoveDango(List<DangoColor> dangos, DangoUIScript dangoUIScript)
        {
            _dangos = dangos;
            _dangoUIScript = dangoUIScript;
        }

        //団子弾(取り外し)
        public void Remove()
        {
            //串に何もなかったら実行しない。
            if (_dangos.Count == 0) return;

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
    }
}
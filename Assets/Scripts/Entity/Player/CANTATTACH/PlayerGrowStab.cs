using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace TM.Entity.Player
{
    class PlayerGrowStab
    {
        const int MAX_DANGO = 7;
        const int GROW_STAB_FRAME = 500;

        private int _currentFrame = GROW_STAB_FRAME;

        /// <summary>
        /// 串が一定時間で伸びる処理
        /// </summary>
        public bool CanGrowStab(int currentMaxValue)
        {
            //刺せる団子の数が最大値だったら実行しない。
            if (currentMaxValue == MAX_DANGO) return false;
            if (--_currentFrame > 0) return false;

            //マイナス値にさせない（ずっと放置してたらいずれintの範囲外になる可能性があるから）
            _currentFrame = 0;
            return true;
        }

        public int GrowStab(int stabMax)
        {
            _currentFrame = GROW_STAB_FRAME;
            return ++stabMax;
        }
    }
}
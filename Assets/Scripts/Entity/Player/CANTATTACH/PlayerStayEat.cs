using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TM.Entity.Player
{
    class PlayerStayEat
    {
        const int STAY_FRAME = 100;

        private int _currentFrame = STAY_FRAME;

        PlayerData _playerData;

        public PlayerStayEat(PlayerData player)
        {
            _playerData = player;
        }

        public bool CanEat()
        {
            //チャージ中に制限時間がゼロになったら弾く
            if (_playerData.GetSatiety() <= 0) return false;

            if (--_currentFrame > 0) return false;

            return true;
        }

        public void ResetCount()
        {
            _currentFrame = STAY_FRAME;
        }
    }
}
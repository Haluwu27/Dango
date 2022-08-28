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
        private int _stayFrame;
        private int _currentFrame;

        public PlayerStayEat(int stayFrame)
        {
            _stayFrame = stayFrame;
            _currentFrame = stayFrame;
        }

        public bool CanEat()
        {
            if (--_currentFrame > 0) return false;

            return true;
        }

        public void ResetCount()
        {
            _currentFrame = _stayFrame;
        }
    }
}
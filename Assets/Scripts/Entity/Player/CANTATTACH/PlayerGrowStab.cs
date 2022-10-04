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

        public int GrowStab(int stabMax)
        {
            if (stabMax >= MAX_DANGO) return MAX_DANGO;

            return ++stabMax;
        }
    }
}
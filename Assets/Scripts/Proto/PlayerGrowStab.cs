using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

class PlayerGrowStab
{
    private int _maxValue = 0;
    private int _growFrame = 0;
    private int _currentFrame = 0;

    public PlayerGrowStab(int maxValue,int growFrame)
    {
        _maxValue = maxValue;
        _growFrame = growFrame;
        _currentFrame = growFrame;
    }

    /// <summary>
    /// 串が一定時間で伸びる処理
    /// </summary>
    public bool CanGrowStab(int currentMaxValue)
    {
        //刺せる団子の数が最大値だったら実行しない。
        if (currentMaxValue == _maxValue) return false;
        if (--_currentFrame > 0) return false;

        //マイナス値にさせない（ずっと放置してたらいずれintの範囲外になる可能性があるから）
        _currentFrame = 0;
        return true;
    }

    public int GrowStab(int stabMax)
    {
        _currentFrame = _growFrame;
        return ++stabMax;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum RoleType
{
    None,
    Buff,
    Debuff,
    Attack
}

public enum BuffType
{
    AttackUp = 1,
    MotionSpdUp,
    HpUp,
    JumpPowerUp,
    MoveSpdUp,
    StrengthUp,
    DebuffUp,
}

public enum DebuffType
{
    AttackDown = 1,
    MotionSpdDown,
    HpDown,
    JumpPowerDown,
    MoveSpdDown,
    StrengthDown,
    DebuffDown,
}

public class DangoRole : MonoBehaviour
{
    private static RoleType CheckRoleType(DangoType[] types)
    {
        //重複を除去した長さを判定。null除外ができればいいのに。というわけで全部刺さってた場合バグります。
        if (types.Distinct().Count() == 2)
            return RoleType.Buff;
        else if (types.Distinct().Count() == 3)
            return RoleType.Debuff;
        else if (types.Distinct().Count() >= 4)
            return RoleType.Attack;
        else return RoleType.None;
    }

    public static RoleType CheckRole(DangoType[] types, out float returnVal)
    {
        switch (CheckRoleType(types))
        {
            case RoleType.Buff:
                BuffType type = (BuffType)types[0];
                returnVal = type switch
                {
                    BuffType.AttackUp => types.Count(),
                    BuffType.MotionSpdUp => types.Count(),
                    BuffType.HpUp => types.Count(),
                    BuffType.JumpPowerUp => types.Count(),
                    BuffType.MoveSpdUp => types.Count(),
                    BuffType.StrengthUp => types.Count(),
                    BuffType.DebuffUp => types.Count(),
                    _ => 0
                };
                return RoleType.Buff;

            case RoleType.Debuff:
                returnVal = 0f;
                return RoleType.Debuff;

            case RoleType.Attack:
                float damage = types.Count();
                returnVal = damage;
                return RoleType.Attack;
        }
        returnVal = 0f;
        return RoleType.None;
    }
}

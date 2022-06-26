using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DangoRoleUI
{
    private static string _currentRoleName;

    public static void OnGUIRoleName(string role_name,float score)
    {
        _currentRoleName = "「" + role_name + "」！" + score + "点！";
    }

    public static void OnGUIReset()
    {
        _currentRoleName = "";
    }

    public static string CurrentRoleName => _currentRoleName;
}

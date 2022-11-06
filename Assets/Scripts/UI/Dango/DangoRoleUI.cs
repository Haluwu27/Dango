using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class DangoRoleUI
{
    private static string _currentRoleName;

    public static void OnGUIRoleName(string role_name)
    {
        _currentRoleName = "u" + role_name + "v";
    }

    public static void OnGUIReset()
    {
        _currentRoleName = "";
    }

    public static string CurrentRoleName => _currentRoleName;
}

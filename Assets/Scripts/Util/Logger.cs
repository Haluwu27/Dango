using System.Diagnostics;

public static class Logger
{
    [Conditional("UNITY_EDITOR")]
    public static void Log(object obj)
    {
        UnityEngine.Debug.Log(obj);
    }

    [Conditional("UNITY_EDITOR")]
    public static void Warn(object obj)
    {
        UnityEngine.Debug.LogWarning(obj);
    }

    [Conditional("UNITY_EDITOR")]
    public static void Error(object obj)
    {
        UnityEngine.Debug.LogError(obj);
    }

    [Conditional("UNITY_EDITOR")]
    public static void Assert(bool obj)
    {
        UnityEngine.Debug.Assert(obj);
    }
}

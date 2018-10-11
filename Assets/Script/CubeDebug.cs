
using UnityEngine;

public static class CubeDebug  {

    public enum DebugLogType
    {
        info,
        warning,
        error
    }

    public static void Log(int _height, int _column, string _reason,DebugLogType _type)
    {
        switch (_type)
        {
            case DebugLogType.info:
                Debug.Log("行:" + _height + " 列:" + _column + " 原因:"+_reason);
                break;
            case DebugLogType.warning:
                Debug.LogWarning("行:" + _height + " 列:" + _column + " 原因:" + _reason);
                break;
            case DebugLogType.error:
                Debug.LogError("行:" + _height + " 列:" + _column + " 原因:" + _reason);
                break;
        }
    }
}

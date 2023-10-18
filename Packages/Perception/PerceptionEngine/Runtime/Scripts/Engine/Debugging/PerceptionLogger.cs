using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Object = UnityEngine.Object;

namespace Perception.Engine
{
    /// <summary>
    /// A class which gives us some nice formatted logging for our objects.
    /// </summary>
    public static class PerceptionLogger
    {
        private static void DoLog(Action<string, Object> LogFunction, string prefix, Object obj, params object[] msg)
        {
#if UNITY_EDITOR
            LogFunction($"{prefix}[<color=lightblue>{obj.name}</color>]: {String.Join(";", msg)}", obj);
#endif
        }

        public static void Log(this Object obj, params object[] msg)
        {
            DoLog(Debug.Log, "", obj, msg);
        }

        public static void LogError(this Object obj, params object[] msg)
        {
            DoLog(Debug.LogError, "<color=red><!></color>", obj, msg);
        }

        public static void LogWarning(this Object obj, params object[] msg)
        {
            DoLog(Debug.LogWarning, "⚠️", obj, msg);
        }

        public static void LogSuccess(this Object obj, params object[] msg)
        {
            DoLog(Debug.Log, "<color=green><!></color>", obj, msg);
        }


    }
}

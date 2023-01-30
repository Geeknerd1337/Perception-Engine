using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Perception.Engine
{
    public class LogicData : LevelEntity
    {
        public List<LogicDataEntry> DataEntries = new List<LogicDataEntry>();
    }

    [System.Serializable]
    /// <summary>
    /// Originally I did some hack to serialize an object, but it was essentially the same thing as below and this is more reliable
    /// and easy to modify in a custom property drawer
    /// </summary>
    public class LogicDataEntry
    {
        public string Key = "Data Key";

        public string StringValue;
        public int IntValue;
        public float FloatValue;
        public bool BoolValue;
        public Vector3 Vector3Value;

        public LogicDataEntryType Type;

        public object GetValue()
        {
            switch (Type)
            {
                case LogicDataEntryType.String:
                    return StringValue;
                case LogicDataEntryType.Int:
                    return IntValue;
                case LogicDataEntryType.Float:
                    return FloatValue;
                case LogicDataEntryType.Bool:
                    return BoolValue;
                case LogicDataEntryType.Vector3:
                    return Vector3Value;
                default:
                    return null;
            }
        }


    }

    public enum LogicDataEntryType
    {
        String,
        Int,
        Float,
        Bool,
        Vector3,
    }
}

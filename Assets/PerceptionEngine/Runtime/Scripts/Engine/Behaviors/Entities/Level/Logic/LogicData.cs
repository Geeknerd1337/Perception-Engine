using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Perception.Engine
{
    /// <summary>
    /// LogicData is a node which essentially contains a dictionary of string object value pairs. It is used to store data you may want to use for various
    /// pieces of level logic. 
    /// 
    /// TODO: This should probably be a property on every entity, but I felt the separation was cleaner from an LD standpoint.
    /// </summary>
    public class LogicData : LevelEntity
    {
        public List<LogicDataEntry> DataEntries = new List<LogicDataEntry>();


        //OnChanged called when set value is called
        public Action<LogicData> OnChanged;

        //Set a value in the data entries
        public void SetValue(string key, object value)
        {
            OnChanged?.Invoke(this);
            foreach (var entry in DataEntries)
            {
                if (entry.Key == key)
                {
                    if (entry.Type == LogicDataEntryType.String)
                    {
                        entry.StringValue = (string)value;
                    }
                    else if (entry.Type == LogicDataEntryType.Int)
                    {
                        entry.IntValue = (int)value;
                    }
                    else if (entry.Type == LogicDataEntryType.Float)
                    {
                        entry.FloatValue = (float)value;
                    }
                    else if (entry.Type == LogicDataEntryType.Bool)
                    {
                        entry.BoolValue = (bool)value;
                    }
                    else if (entry.Type == LogicDataEntryType.Vector3)
                    {
                        entry.Vector3Value = (Vector3)value;
                    }
                }
            }
        }
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

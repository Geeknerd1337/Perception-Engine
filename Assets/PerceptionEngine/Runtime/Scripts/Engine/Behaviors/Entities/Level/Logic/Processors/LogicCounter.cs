using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    public class LogicCounter : LogicDataProcessor
    {
        public string Value;

        public void Increment(int i)
        {
            var entires = Input.DataEntries;

            foreach (var entry in entires)
            {
                if (entry.Key == Value)
                {
                    if (entry.Type == LogicDataEntryType.Int)
                    {
                        Input.SetValue(Value, entry.IntValue + i);
                    }
                    else if (entry.Type == LogicDataEntryType.Float)
                    {
                        Input.SetValue(Value, entry.FloatValue + i);
                    }
                    else
                    {
                        Debug.LogError("LogicCounter: Value is not an int or float");
                    }
                }
            }
        }
    }
}
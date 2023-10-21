using System.Linq;
using UnityEngine;

namespace Perception.Engine
{
    public enum Comparison
    {
        Equal,
        NotEqual,
        Greater,
        Less,
        GreaterOrEqual,
        LessOrEqual,
        DivisableBy
    }

    /// <summary>
    /// Fires its event if the input data entry is equal to the CompareTo data entry.
    /// </summary>
    public class LogicCase : LogicDataProcessor
    {
        [BoxGroup("Events")]
        public string KeyValue;
        /// <summary>
        /// The data entry we are going to compare to. 
        /// </summary>
        [BoxGroup("Events")]
        [Header("Data to Compare")]
        public LogicDataEntry CompareTo;

        [BoxGroup("Events")]
        public Comparison Comparison;

        public override void Start()
        {
            base.Start();

            if (Input != null)
            {
                //Subsribe to the input's data changed event.
                Input.OnChanged += InputChanged;
            }

        }

        private void InputChanged(LogicData obj)
        {
            Evaluate();
        }

        //Evaluate the case and fire it if it is true.
        public void Evaluate()
        {
            if (EntyExists())
            {
                if (Compare())
                {
                    //Fire the case
                    Fire();
                }
            }
            else
            {
                Debug.LogError("LogicCase: Entry does not exist");
            }
        }

        private bool EntyExists()
        {
            return Input.DataEntries.Any(x => x.Key == KeyValue && x.Type == CompareTo.Type);
        }

        private bool Compare()
        {
            var entry = Input.DataEntries.First(x => x.Key == KeyValue && x.Type == CompareTo.Type);
            //Compare the entry based on type
            switch (entry.Type)
            {
                case LogicDataEntryType.Int:
                    return CompareInt(entry.IntValue);
                case LogicDataEntryType.Float:
                    return CompareFloat(entry.FloatValue);
                case LogicDataEntryType.String:
                    return CompareString(entry.StringValue);
                case LogicDataEntryType.Bool:
                    return CompareBool(entry.BoolValue);
                case LogicDataEntryType.Vector3:
                    return CompareVector3(entry.Vector3Value);
                default:
                    return false;
            }


        }

        private bool CompareVector3(Vector3 vector3Value)
        {
            switch (Comparison)
            {
                case Comparison.Equal:
                    return vector3Value == CompareTo.Vector3Value;
                case Comparison.NotEqual:
                    return vector3Value != CompareTo.Vector3Value;
                default:
                    return false;
            }
        }

        private bool CompareBool(bool boolValue)
        {
            switch (Comparison)
            {
                case Comparison.Equal:
                    return boolValue == CompareTo.BoolValue;
                case Comparison.NotEqual:
                    return boolValue != CompareTo.BoolValue;
                default:
                    return false;
            }
        }

        private bool CompareString(string stringValue)
        {
            switch (Comparison)
            {
                case Comparison.Equal:
                    return stringValue == CompareTo.StringValue;
                case Comparison.NotEqual:
                    return stringValue != CompareTo.StringValue;
                default:
                    return false;
            }
        }

        private bool CompareFloat(float floatValue)
        {
            switch (Comparison)
            {
                case Comparison.Equal:
                    return floatValue == CompareTo.FloatValue;
                case Comparison.NotEqual:
                    return floatValue != CompareTo.FloatValue;
                case Comparison.Greater:
                    return floatValue > CompareTo.FloatValue;
                case Comparison.Less:
                    return floatValue < CompareTo.FloatValue;
                case Comparison.GreaterOrEqual:
                    return floatValue >= CompareTo.FloatValue;
                case Comparison.LessOrEqual:
                    return floatValue <= CompareTo.FloatValue;
                case Comparison.DivisableBy:
                    return floatValue % CompareTo.FloatValue == 0;
                default:
                    return false;
            }
        }

        private bool CompareInt(int intValue)
        {
            switch (Comparison)
            {
                case Comparison.Equal:
                    return intValue == CompareTo.IntValue;
                case Comparison.NotEqual:
                    return intValue != CompareTo.IntValue;
                case Comparison.Greater:
                    return intValue > CompareTo.IntValue;
                case Comparison.Less:
                    return intValue < CompareTo.IntValue;
                case Comparison.GreaterOrEqual:
                    return intValue >= CompareTo.IntValue;
                case Comparison.LessOrEqual:
                    return intValue <= CompareTo.IntValue;
                case Comparison.DivisableBy:
                    return intValue % CompareTo.IntValue == 0;
                default:
                    return false;
            }
        }



    }
}

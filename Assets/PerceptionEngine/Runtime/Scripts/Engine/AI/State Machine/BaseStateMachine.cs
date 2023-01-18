using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Perception.Engine.AI
{
    /// <summary>
    /// The basis of a state machine which can be used for controlling an actor.
    /// </summary>
    public class BaseStateMachine : MonoBehaviour
    {
        /// <summary>
        /// Reference to the enmy this state machine belongs to.
        /// </summary>
        public Actor Actor;

        /// <summary>
        /// The current state of the state machine
        /// </summary>
        public BaseState CurrentState;


        [SerializeField] private BaseState _initialState;

        [SerializeField] public StateData StateData = new StateData();

        /// <summary>
        /// A dictionary of cached components that can be accessed by the state machine.
        /// </summary>
        private Dictionary<Type, Component> _cachedComponents;


        [SerializeField] public TimeSince TimeInState;

        /// <summary>
        /// Some states may not need to be updated every frame.
        /// </summary>
        private TimeSince TimeSinceExecute;

        public delegate void EnterStateAction(BaseState state);
        public event EnterStateAction OnStateEntered;

        public delegate void ExecuteStateAction(BaseState state);
        public event ExecuteStateAction OnStateExecuted;

        private void Awake()
        {
            CurrentState = _initialState;
            _cachedComponents = new Dictionary<Type, Component>();
            Actor = GetComponent<Actor>();
        }

        private void Update()
        {
            if (TimeSinceExecute > CurrentState.ThinkRate)
            {
                CurrentState.Execute(this);

                TimeSinceExecute = 0;
            }

            OnStateExecuted?.Invoke(CurrentState);
        }

        public void InvokeStateEntered(BaseState state)
        {
            if (OnStateEntered != null)
            {
                OnStateEntered(state);
            }
        }


        /// <summary>
        /// Custom get component override which will cache the components in the state machine.
        /// </summary>
        public new T GetComponent<T>() where T : Component
        {
            if (_cachedComponents.ContainsKey(typeof(T)))
                return _cachedComponents[typeof(T)] as T;

            var component = base.GetComponent<T>();
            if (component != null)
            {
                _cachedComponents.Add(typeof(T), component);
            }
            return component;
        }

        public void SetState(BaseState state)
        {
            CurrentState = state;
        }
    }

    /// <summary>
    /// Sometimes a class needs to store or access data that would usually require creating a separate class.
    /// This way we can store the data in the state machine and access it from the state itself.
    /// Think of this as being very similar to AnimGraph parameters, but done via code.
    /// </summary>
    [Serializable]
    public class StateData
    {
        public Dictionary<string, object> Data = new Dictionary<string, object>();

        public void SetFloat(string name, float value)
        {
            if (Data.ContainsKey(name))
                Data[name] = value;
            else
                Data.Add(name, value);
        }


        public float GetFloat(string name)
        {
            if (Data.ContainsKey(name))
                return (float)Data[name];
            return 0;
        }

        public void SetInt(string name, int value)
        {
            if (Data.ContainsKey(name))
                Data[name] = value;
            else
                Data.Add(name, value);
        }

        public int GetInt(string name)
        {
            if (Data.ContainsKey(name))
                return (int)Data[name];
            return 0;
        }

        public void SetBool(string name, bool value)
        {
            if (Data.ContainsKey(name))
                Data[name] = value;
            else
                Data.Add(name, value);
        }

        public bool GetBool(string name)
        {
            if (Data.ContainsKey(name))
                return (bool)Data[name];
            return false;
        }

        public void SetString(string name, string value)
        {
            if (Data.ContainsKey(name))
                Data[name] = value;
            else
                Data.Add(name, value);
        }

        public string GetString(string name)
        {
            if (Data.ContainsKey(name))
                return (string)Data[name];
            return null;
        }

        public void SetVector3(string name, Vector3 value)
        {
            if (Data.ContainsKey(name))
                Data[name] = value;
            else
                Data.Add(name, value);
        }

        public Vector3 GetVector3(string name)
        {
            if (Data.ContainsKey(name))
                return (Vector3)Data[name];
            return Vector3.zero;
        }

        public void SetTimeSince(string name, TimeSince value)
        {
            if (Data.ContainsKey(name))
                Data[name] = value;
            else
                Data.Add(name, value);
        }

        public TimeSince GetTimeSince(string name)
        {
            if (Data.ContainsKey(name))
                return (TimeSince)Data[name];
            return new TimeSince();
        }


    }
}

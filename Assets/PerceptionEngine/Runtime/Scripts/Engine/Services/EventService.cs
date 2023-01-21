using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;

namespace Perception.Engine
{
    /// <summary>
    /// Uses event attributes to register and invoke events when EventService.Run is called. Caches all methods with the EventAttribute.
    /// Requires the gameobjects register and unregister themselves with the service, handled automatically on Entities.
    /// TODO: Make all gameObjects register and unregister themselves with the service, also allow parameters to be passed to the event.
    /// </summary>
    public class EventService : PerceptionService
    {
        private Dictionary<string, List<Action>> _eventDictionary = new Dictionary<string, List<Action>>();

        public override void Awake()
        {
            base.Awake();

        }

        public void RunEvents(string eventName)
        {
            if (_eventDictionary.ContainsKey(eventName))
            {
                foreach (Action action in _eventDictionary[eventName])
                {
                    if (action != null)
                    {
                        action.Invoke();
                    }
                    else
                    {
                        //Remove the action from the list
                        _eventDictionary[eventName].Remove(action);
                    }
                }
            }
        }

        public static void Run(string eventName)
        {
            GameManager.GetService<EventService>().RunEvents(eventName);
        }

        public static void Register(MonoBehaviour g)
        {
            GameManager.GetService<EventService>().RegisterGameObject(g);
        }

        public static void Unregister(MonoBehaviour g)
        {
            GameManager.GetService<EventService>().UnregisterGameObject(g);
        }

        public void RegisterGameObject(MonoBehaviour g)
        {
            foreach (MethodInfo method in g.GetType().GetMethods())
            {

                foreach (Attribute attribute in method.GetCustomAttributes(true))
                {
                    if (attribute is EventAttribute)
                    {
                        EventAttribute eventAttribute = (EventAttribute)attribute;
                        if (!_eventDictionary.ContainsKey(eventAttribute.EventName))
                        {
                            _eventDictionary.Add(eventAttribute.EventName, new List<Action>());
                        }
                        _eventDictionary[eventAttribute.EventName].Add((Action)Delegate.CreateDelegate(typeof(Action), g, method));
                    }
                }
            }
        }

        public void UnregisterGameObject(MonoBehaviour g)
        {
            //Removes all actions from the dictionary that are bound to the gameobject
            foreach (KeyValuePair<string, List<Action>> entry in _eventDictionary)
            {
                bool deleted = false;
                foreach (Action action in entry.Value)
                {
                    if ((object)action.Target == g)
                    {
                        entry.Value.Remove(action);
                        break;
                    }
                }
                if (deleted)
                {
                    break;
                }
            }

            //Remove any empty lists
            List<string> keysToRemove = new List<string>();
            foreach (KeyValuePair<string, List<Action>> entry in _eventDictionary)
            {
                if (entry.Value.Count == 0)
                {
                    keysToRemove.Add(entry.Key);
                }
            }
            foreach (string key in keysToRemove)
            {
                _eventDictionary.Remove(key);
            }

        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class EventAttribute : Attribute
    {
        public string EventName;
        public EventAttribute(string eventName)
        {
            EventName = eventName;
        }
    }
}

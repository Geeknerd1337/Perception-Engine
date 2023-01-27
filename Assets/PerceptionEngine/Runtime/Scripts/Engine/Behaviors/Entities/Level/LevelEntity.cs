using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Perception.Engine
{
    /// <summary>
    /// A level entity is an entity that is part of the level which can fire events.
    /// </summary>
    public class LevelEntity : Entity
    {
        public virtual string Icon => "LevelEntity.png";

        public bool DrawGizmos;

        public int Test;


        [BoxGroup("Events")]
        public UnityEvent OnFired;

        public virtual void OnDrawGizmos()
        {
            if (DrawGizmos)
            {
                Gizmos.DrawIcon(transform.position, Icon, true);
                DrawLinesToEvent(OnFired);
            }
        }


        //A method which draws an arrow to every event listener
        public void DrawLinesToEvent(UnityEvent eventToDrawTo)
        {
            //Old gizmos color
            var oldColor = Gizmos.color;
            Gizmos.color = Color.yellow.WithAlpha(0.5f);
            for (int i = 0; i < eventToDrawTo.GetPersistentEventCount(); i++)
            {
                var obj = eventToDrawTo.GetPersistentTarget(i);

                if (obj is MonoBehaviour)
                {
                    var mono = obj as MonoBehaviour;
                    var pos = mono.transform.position;
                    var rot = mono.transform.rotation;
                    DrawArrow(transform.position, pos);
                }
                //If obj is a transform or a gameobject
                if (obj is Transform t)
                {
                    var pos = t.position;
                    var rot = t.rotation;
                    DrawArrow(transform.position, pos);
                }

                if (obj is GameObject g)
                {
                    var pos = g.transform.position;
                    var rot = g.transform.rotation;
                    DrawArrow(transform.position, pos);
                }
            }
            Gizmos.color = oldColor;
        }


        public static void DrawArrow(Vector3 start, Vector3 end, float arrowHeadLength = 1f, float arrowHeadAngle = 145f)
        {
            Gizmos.DrawLine(start, end);

            Vector3 right = Quaternion.LookRotation(end - start) * Quaternion.Euler(arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(end - start) * Quaternion.Euler(-arrowHeadAngle, 0, 0) * new Vector3(0, 0, 1);
            Gizmos.DrawRay(end, right * arrowHeadLength);
            Gizmos.DrawRay(end, left * arrowHeadLength);
        }


    }
}

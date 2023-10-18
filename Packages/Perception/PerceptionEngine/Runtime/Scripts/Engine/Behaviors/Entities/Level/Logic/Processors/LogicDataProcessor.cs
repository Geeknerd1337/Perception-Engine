using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    /// <summary>
    /// A data processor is a level entity that processes and performs operations on LogicData nodes.
    /// </summary>
    public class LogicDataProcessor : LevelEntity
    {
        public LogicData Input;

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (DrawGizmos)
            {
                //Draw blue error from our input to us
                if (Input != null)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(transform.position, Input.transform.position);
                }
            }
        }

    }
}


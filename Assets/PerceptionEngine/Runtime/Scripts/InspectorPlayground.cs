using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Perception.Engine;

/// <summary>
/// A playground class I'm using to test my editor code.
/// </summary>
public class InspectorPlayground : Entity
{

    [Tab("Other")]
    public float TestVariable;

    [BoxGroup("Hey")]
    public float Test2;

    [BoxGroup("Hey")]
    public float Test3;

    [BoxGroup("Hey")]
    public float Hey;

    [BoxGroup("Hey")]
    public List<int> TestList;

    [BoxGroup("Hey")]
    public UnityEvent TestEvent;

    public UnityEvent Event2;


    [BoxGroup("Vectors")]
    [MinMaxSlider(0, 10)]
    [Tab("Misc")]
    public Vector2 TestVector2;


    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerceptionAudio.FromWorld("Bark", this.transform.position);
        }
    }

}


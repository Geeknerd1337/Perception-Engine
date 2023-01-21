using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Perception.Engine;


public class Test : Entity
{
    [Event("Bark")]
    public void Tester()
    {
        this.Log("BARK!");
    }
}


using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct MovementData : IComponentData
{
    public float maxMoveSpeed;
    public float acceleration;
    public float maxTurnSpeed;
    public float angularAccleration;
}

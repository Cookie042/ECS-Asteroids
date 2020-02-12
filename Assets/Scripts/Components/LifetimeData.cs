using System;
using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct LifetimeData : IComponentData
{
    public float lifetime;
}
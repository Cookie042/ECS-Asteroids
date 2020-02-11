using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[GenerateAuthoringComponent]
public struct SpawnerData : IComponentData
{
    public Entity prefab;
}

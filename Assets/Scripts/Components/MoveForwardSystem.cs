using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MoveForwardSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var dt = Time.DeltaTime;
        inputDeps = Entities.ForEach((ref Translation pos, in Rotation rot, in MoveForward moveFor ) =>
        {
            var worldForward = math.mul(rot.Value, new float3(0, 0, 1));
            pos.Value += worldForward * moveFor.speed * dt;
        }).Schedule(inputDeps);
        
        return inputDeps;
    }
}

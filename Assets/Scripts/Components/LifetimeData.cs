using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct LifetimeData : IComponentData
{
    public float timeToLive;
}

public class LifetimeSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var dt = Time.DeltaTime;
        
        var ecb = new EntityCommandBuffer(Allocator.TempJob);
        
        Entities.ForEach((Entity e, ref LifetimeData lifetime) =>
        {
            lifetime.timeToLive -= dt;

            if (lifetime.timeToLive <= 0)
            {
                ecb.DestroyEntity(e);
            }
        }).Run();
        
        ecb.Playback(EntityManager);
        ecb.Dispose();

        return default;
    }
}


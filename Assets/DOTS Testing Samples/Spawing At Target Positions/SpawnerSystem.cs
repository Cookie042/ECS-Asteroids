using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class SpawnerSystem : JobComponentSystem
{
    private EntityCommandBufferSystem _entityCommandBufferSystem;

    protected override void OnCreate()
    {
        _entityCommandBufferSystem =
            World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();

        var childBuffer = _entityCommandBufferSystem.GetBufferFromEntity<Child>();
        
        Entities.WithoutBurst().ForEach((int entityInQueryIndex, in Entity e, in SpawnerData spawnerdata) =>
        {
            
        }).Run();
        
        return default;
    }
}
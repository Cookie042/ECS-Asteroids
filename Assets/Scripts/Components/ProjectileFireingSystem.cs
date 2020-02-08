using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class ProjectileFireingSystem : JobComponentSystem
{
    private EntityCommandBufferSystem _ecbSystem;
    protected override void OnCreate()
    {
        _ecbSystem = World.GetOrCreateSystem<EntityCommandBufferSystem>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var em = World.EntityManager;
        var dt = Time.DeltaTime;

        var _jobecb = _ecbSystem.CreateCommandBuffer().ToConcurrent();
        
        var handle = Entities.ForEach(
            (int nativeThreadIndex, ref PlayerShootingData shootingData, in Translation pos ,in Rotation rotation) =>
            {
                if (!shootingData.isFiring) return;

                shootingData.timeSinceLastShot += dt;

                if (shootingData.timeSinceLastShot > shootingData.fireRate)
                {
                    shootingData.timeSinceLastShot -= shootingData.fireRate;

                     for (int i = 0; i < 1000000; i++)
                    {
                        Entity newBullet = _jobecb.Instantiate(nativeThreadIndex, shootingData.BulletPrefab);
                        
                        
                        _jobecb.SetComponent(nativeThreadIndex, newBullet, new Translation() {Value = pos.Value + math.mul(rotation.Value, new float3(0, 0, 1))});
                        _jobecb.SetComponent(nativeThreadIndex, newBullet, new Rotation() {Value = rotation.Value});
                    }
                }
            }).Schedule(inputDeps);
        
        _ecbSystem.AddJobHandleForProducer(handle);

        return handle;
    }
}
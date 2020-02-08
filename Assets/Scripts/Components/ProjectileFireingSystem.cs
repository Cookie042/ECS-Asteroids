using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

[AlwaysSynchronizeSystem]
public class ProjectileFireingSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var em = World.EntityManager;
        var dt = Time.DeltaTime;
        
        Entities.WithoutBurst().WithStructuralChanges().ForEach(
            (ref PlayerShootingData shootingData, in Translation pos, in Rotation rotation) =>
            {
                if (!shootingData.isFiring) return;

                shootingData.timeSinceLastShot += dt;

                if (shootingData.timeSinceLastShot > shootingData.fireRate)
                {
                    shootingData.timeSinceLastShot -= shootingData.fireRate;

                     for (int i = 0; i < 100000; i++)
                    {
                        Entity newBullet = EntityManager.Instantiate(shootingData.BulletPrefab);

                        EntityManager.SetComponentData(newBullet,
                            new Translation() {Value = pos.Value + math.mul(rotation.Value, new float3(0, 0, 1))});
                        EntityManager.SetComponentData(newBullet,
                            new Rotation() {Value = rotation.Value});
                    }
                }
            }).Run();


        return default;
    }
}
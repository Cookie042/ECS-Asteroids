using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerShootingData : IComponentData
{
    public Entity BulletPrefab;
    public float timeSinceLastShot;
    public float fireRate;
    public bool isFiring;
}
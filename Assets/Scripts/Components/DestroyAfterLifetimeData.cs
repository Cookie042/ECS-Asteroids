using Unity.Entities;

[GenerateAuthoringComponent]
public struct DestroyAfterLifetimeData : IComponentData
{
    public float age;
}
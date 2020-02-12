using Unity.Entities;

[GenerateAuthoringComponent]
public struct MoveForwardData : IComponentData
{
    public float speed;
}
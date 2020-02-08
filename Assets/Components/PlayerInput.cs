using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct PlayerInput : IComponentData
{
    public float2 inputDirection;
    public float fireing;
}

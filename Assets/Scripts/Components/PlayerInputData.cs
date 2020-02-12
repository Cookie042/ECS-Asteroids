using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct PlayerInputData : IComponentData
{
    public float2 inputDirection;
    public float fireing;
}

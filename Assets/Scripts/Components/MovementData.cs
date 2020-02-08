using Unity.Entities;

[GenerateAuthoringComponent]
public struct MovementData : IComponentData
{
    public float maxMoveSpeed;
    public float acceleration;
    public float maxTurnSpeed;
    public float angularAccleration;
}

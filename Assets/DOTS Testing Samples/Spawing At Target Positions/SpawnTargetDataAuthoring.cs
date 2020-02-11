using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SpawnTargetDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var parent = dstManager.GetComponentData<Parent>(entity);

        var ltwBuffer = conversionSystem.GetComponentDataFromEntity<LocalToWorld>();

        dstManager.AddComponentData(entity, new SpawnTargetData());
        
        DynamicBuffer<SpawnTargetElementData> buffer;
        if (dstManager.HasComponent<SpawnTargetElementData>(parent.Value) 
            && dstManager.HasComponent<SpawnerData>(parent.Value))
            buffer = dstManager.GetBuffer<SpawnTargetElementData>(parent.Value);
        else
            buffer = dstManager.AddBuffer<SpawnTargetElementData>(parent.Value);

        buffer.Add(new SpawnTargetElementData() {Value = entity});
    }
}

public struct SpawnTargetData : IComponentData
{
    public SpawnTargetData(int i)
    {
        
    }
}

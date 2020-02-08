using TMPro;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class PlayerMovementSystem : JobComponentSystem
{
    private TMP_Text _text;

    protected override void OnCreate()
    {
        _text = GameObject.FindObjectOfType<TMP_Text>();
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var dt = Time.DeltaTime;

        Entities.WithoutBurst().ForEach((ref PhysicsVelocity vel , in LocalToWorld ltw, in Rotation rot, in PlayerInput input, in MovementData moveData) =>
        {
            var localMovement = new float3(0, 0, moveData.acceleration * input.inputDirection.y) * dt;
            var pos = math.transform(ltw.Value, float3.zero);
            
            //Debug.DrawRay(pos, math.mul(rot.Value, new float3(0,0,10)));

            
            if (_text != null) _text.text = pos.ToString();

            vel.Linear += math.mul(rot.Value, localMovement);
            
            vel.Angular += new float3(0,moveData.angularAccleration * input.inputDirection.x,0) * dt;

            // if (math.length(vel.Linear) > moveData.maxMoveSpeed)
            //     vel.Linear = math.normalize(vel.Linear) * moveData.maxMoveSpeed;
            //
            // //Debug.Log(math.length(vel.Linear));
            // if (math.length(vel.Angular) > moveData.maxTurnSpeed)
            //     vel.Angular = math.normalize(vel.Angular) * moveData.maxTurnSpeed;
        }).Run();

        return default;
    }
}

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;

[AlwaysSynchronizeSystem]
public class PlayerInputSystem : JobComponentSystem
{
    private AstroidsInput _inputActions;

    protected override void OnCreate()
    {
        if (_inputActions == null)
        {
            _inputActions = new AstroidsInput();
            
            _inputActions.Player.Enable();
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        //Debug.Log(_inputActions.Player.Move.ReadValue<Vector2>());
        
        Entities.WithoutBurst().ForEach((ref PlayerInput pinput) =>
        {
            var moveValue = _inputActions.Player.Move.ReadValue<Vector2>();
            var fireValue = _inputActions.Player.Fire.ReadValue<float>();
            pinput.inputDirection = moveValue;
            pinput.fireing = fireValue;
        }).Run();

        Entities.WithoutBurst().ForEach((ref PlayerShootingData shootData, in PlayerInput input) =>
            {
                shootData.isFiring = input.fireing > 0.5f;
            }).Run();

        return default;
    }
}
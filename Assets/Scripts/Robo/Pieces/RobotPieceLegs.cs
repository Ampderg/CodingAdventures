using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RobotPiece_Legs_", menuName = "ScriptableObjects/RobotPieces/Legs")]
public class RobotPieceLegs : RobotPiece
{
    [System.Serializable]
    public class MovementSettings
    {
        [Header("Lateral Movement")]
        public float MaxSpeed = 14;
        public float Acceleration;
        public float Deceleration;

        [Header("Vertical Movement")]
        public float TimeUntilMaxCharge = 0;

        public float VerticalImpulseScale = 20f;
        public AnimationCurve VerticalImpulseOverCharge = AnimationCurve.Linear(0, 0, 1, 1);

        public float LateralImpulseScale = 0f;
        public AnimationCurve LateralImpulseOverCharge = AnimationCurve.Constant(0, 1, 0);

        
    }

    public MovementSettings GroundedSettings;
    public MovementSettings AerialSettings;

    public bool DoesCharge = true;

    public float FallGravity = 20f;
    public float JumpHoldGravity = 5f;
    public float JumpNonholdGravity = 10f;

    public float MaxFallSpeed = 20f;


    public bool CanFloat = true;
    public float FloatWindupSpeed = 1f;

    public float FloatUpwardsVelocity = 0;

    public override RobotSlot GetSlotType()
    {
        return RobotSlot.Legs;
    }
}

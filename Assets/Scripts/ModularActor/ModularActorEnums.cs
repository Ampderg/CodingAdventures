using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActorModuleType
{
    AlwaysTick,
    GenericMovement,
    GroundedLateralMovement,
    AerialLateralMovement,
    VerticalMovement,
    CharacterRotation,
    MeshRotation,
    CameraRotation,
    CameraOffset,
    GroundedChecker
}

public enum ModularActorVariables_Bool
{
    Paused,
    Input_Pause,
    Input_JumpHeld,
    Input_JumpDown,
    Input_JumpUp,
    Grounded,
    Floating
}

public enum ModularActorVariables_Float
{
    Input_Move_X,
    Input_Move_Y,
    Input_Camera_X,
    Input_Camera_Y,
    MaxSpeed,
    OverrideDeceleration
}

public enum ModularActorVariables_Object
{
    CharacterRigidbody,
    RobotFrame
}
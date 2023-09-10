using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRobo_Logic_Player : ModularActorLogic
{

    [SerializeField]
    private new Rigidbody rigidbody;
    [SerializeField]
    private RobotFrame frame;
    protected override void ProcessInput()
    {
        base.ProcessInput();

        SetFloat(ModularActorVariables_Float.Input_Move_X, Input.GetAxisRaw("Horizontal"));
        SetFloat(ModularActorVariables_Float.Input_Move_Y, Input.GetAxisRaw("Vertical"));

        //Mouse movement
        SetFloat(ModularActorVariables_Float.Input_Camera_X, Input.GetAxisRaw("Mouse X") * SettingsHandler.CameraSensitivity);
        SetFloat(ModularActorVariables_Float.Input_Camera_Y, Input.GetAxisRaw("Mouse Y") * (SettingsHandler.InvertY ? 1 : -1) * SettingsHandler.CameraSensitivity);

#if UNITY_EDITOR
        SetBool(ModularActorVariables_Bool.Input_Pause, Input.GetKeyDown(KeyCode.Comma));
#else
        SetBool(ModularActorVariables_Bool.Input_Pause, Input.GetKeyDown(KeyCode.Escape));
#endif

        SetBool(ModularActorVariables_Bool.Input_JumpHeld, Input.GetButton("Jump"));

        if (Input.GetButtonDown("Jump"))
            SetBool(ModularActorVariables_Bool.Input_JumpDown, true);

        if (Input.GetButtonUp("Jump"))
            SetBool(ModularActorVariables_Bool.Input_JumpUp, true);
    }

    private void ClearInputDowns()
    {
        SetBool(ModularActorVariables_Bool.Input_JumpDown, false);
        SetBool(ModularActorVariables_Bool.Input_JumpUp, false);
    }

    public override void TryInit()
    {

        frame.BuildRobotMesh();

        SetObject(ModularActorVariables_Object.CharacterRigidbody, rigidbody);
        SetObject(ModularActorVariables_Object.RobotFrame, frame);


        base.TryInit();

        parent.ProcessTick(ActorModuleType.GenericMovement, ModularActor.TickPhase.Init);
        parent.ProcessTick(ActorModuleType.VerticalMovement, ModularActor.TickPhase.Init);
    }

    public override void TryUpdate()
    {
        base.TryUpdate();
        if (!GameHandler.Paused)
        {
            parent.ProcessTick(ActorModuleType.CameraRotation, ModularActor.TickPhase.Update);
            parent.ProcessTick(ActorModuleType.MeshRotation, ModularActor.TickPhase.Update);
        }
    }

    public override void TryFixedUpdate()
    {
        base.TryFixedUpdate();

        if (!GameHandler.Paused)
        {
            parent.ProcessTick(ActorModuleType.CharacterRotation, ModularActor.TickPhase.FixedUpdate);
            parent.ProcessTick(ActorModuleType.CameraOffset, ModularActor.TickPhase.FixedUpdate);

            parent.ProcessTick(ActorModuleType.GroundedChecker, ModularActor.TickPhase.FixedUpdate);

            parent.ProcessTick(ActorModuleType.GenericMovement, ModularActor.TickPhase.FixedUpdate);
            parent.ProcessTick(ActorModuleType.VerticalMovement, ModularActor.TickPhase.FixedUpdate);
        }

        ClearInputDowns();
    }
}

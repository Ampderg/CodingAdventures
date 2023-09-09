using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRobo_Module_MenuHandler : BaseActorModule
{
    private bool paused = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void OnUpdate(ModularActor actor, ModularActorLogic logic)
    {
        bool pausePressed = logic.GetBool(ModularActorVariables_Bool.Input_Pause);

        if(pausePressed)
        {
            paused = !paused;

            if(paused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }    
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            GameHandler.Paused = paused;

            GameHandler.PauseEventArgs args = new GameHandler.PauseEventArgs();
            args.IsPaused = paused;
            GameHandler.OnPauseChanged.Invoke(this, args);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRobo_Module_CharacterRotation : BaseActorModule
{
    [SerializeField]
    private Transform rotationTarget;
    [SerializeField]
    private Transform characterTransform;

    protected override void OnFixedUpdate(ModularActor actor, ModularActorLogic logic)
    {
        characterTransform.rotation = Quaternion.LookRotation(rotationTarget.position - characterTransform.position, Vector3.up);
    }
}

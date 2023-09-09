using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRobo_Module_SmoothMeshRotation : BaseActorModule
{
    [SerializeField]
    private float rotationSpeed = 90;
    [SerializeField]
    private Transform rotationTarget;
    [SerializeField]
    private Transform characterFollow;
    [SerializeField]
    private Transform meshTransform;

    protected override void OnUpdate(ModularActor actor, ModularActorLogic logic)
    {

        meshTransform.position = characterFollow.position;
        meshTransform.forward = Vector3.Slerp(meshTransform.forward, rotationTarget.position - meshTransform.position, rotationSpeed * Time.deltaTime);

    }
}

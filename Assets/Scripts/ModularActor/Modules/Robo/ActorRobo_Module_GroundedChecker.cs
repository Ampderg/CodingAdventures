using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRobo_Module_GroundedChecker : BaseActorModule
{
    [SerializeField]
    private Transform groundedSpherePoint;
    [SerializeField]
    private float overlapSphereRadius = 0.5f;
    [SerializeField]
    private LayerMask collisionMask;
    [SerializeField]
    private float maxYVelocityForGrounded = 0;

    private new Rigidbody rigidbody;

    protected override void OnFixedUpdate(ModularActor actor, ModularActorLogic logic)
    {
        bool grounded = false;

        rigidbody = (Rigidbody)logic.GetObject(ModularActorVariables_Object.CharacterRigidbody);

        Collider[] cols = Physics.OverlapSphere(groundedSpherePoint.position, overlapSphereRadius, collisionMask);
        if(cols.Length > 0)
        {
            //Dont set yourself as grounded if you're leaving the ground!
            //if(rigidbody.velocity.y <= maxYVelocityForGrounded)
            {
                grounded = true;
            }
        }

        logic.SetBool(ModularActorVariables_Bool.Grounded, grounded);
        Debug.Log(grounded);
    }
}

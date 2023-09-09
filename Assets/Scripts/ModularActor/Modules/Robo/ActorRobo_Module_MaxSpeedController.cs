using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRobo_Module_MaxSpeedController : BaseActorModule
{
    [SerializeField]
    private float maxSpeed = 14;
    [SerializeField]
    private LayerMask collisionMask;
    [SerializeField]
    private float sphereCastRadius = 1;
    [SerializeField]
    private float paddingRange = 5f;
    [SerializeField]
    private float minPaddingSpeed = 1f;
    [SerializeField]
    private float paddingDecelerationOverride = 1000f;
    [SerializeField]
    private bool dontSquishFalling = true;

    private new Rigidbody rigidbody;

    protected override void OnInit(ModularActor actor, ModularActorLogic logic)
    {
        rigidbody = (Rigidbody)logic.GetObject(ModularActorVariables_Object.CharacterRigidbody);

        logic.SetFloat(ModularActorVariables_Float.MaxSpeed, maxSpeed);
    }

    protected override void OnFixedUpdate(ModularActor actor, ModularActorLogic logic)
    {

        RaycastHit[] hits = Physics.SphereCastAll(rigidbody.transform.position, sphereCastRadius, rigidbody.velocity.normalized, paddingRange, collisionMask);

        if (hits.Length > 0)
        {
            Vector3 velocity = rigidbody.velocity;
            foreach (RaycastHit hit in hits)
            {
                //Line of sight check
                RaycastHit lineHit; 
                if (!Physics.Linecast(rigidbody.position, hit.point, out lineHit, collisionMask) || lineHit.point == hit.point)
                {
                    if (hit.distance > 0)
                    {
                        //Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);
                        Vector3 velocityAlongCollisionNormal = Vector3.Project(velocity, hit.normal);
                        Vector3 remainingVelocity = velocity - velocityAlongCollisionNormal;

                        if(dontSquishFalling)
                        {
                            Vector3 fallingComponent = Vector3.Project(velocityAlongCollisionNormal, Vector3.down);

                            if(Vector3.Dot(fallingComponent, Vector3.down) > 0)
                            {
                                remainingVelocity += fallingComponent;
                                velocityAlongCollisionNormal -= fallingComponent;
                            }
                        }

                        float factor = 1 - (Mathf.Clamp(hit.distance, 0, paddingRange) / paddingRange);
                        velocityAlongCollisionNormal = velocityAlongCollisionNormal.normalized
                            * Mathf.Min(velocityAlongCollisionNormal.magnitude, Mathf.Lerp(maxSpeed, minPaddingSpeed, factor));

                        velocity = velocityAlongCollisionNormal + remainingVelocity;
                    }
                }
            }

            rigidbody.velocity = velocity;
        }

    }
}

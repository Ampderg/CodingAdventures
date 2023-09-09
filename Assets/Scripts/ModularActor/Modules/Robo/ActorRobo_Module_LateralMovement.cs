using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRobo_Module_LateralMovement : BaseActorModule
{

    [SerializeField]
    private float turnaroundAccelerationMultiplier;
    [SerializeField]
    private float minTurnaroundCurrentVelocity;
    [SerializeField]
    private float minTurnaroundTargetVelocity;
    [SerializeField]
    private float turnaroundDotAngle = 0;

    private new Rigidbody rigidbody;
    private RobotFrame frame;
    

    protected override void OnInit(ModularActor actor, ModularActorLogic logic)
    {
        rigidbody = (Rigidbody)logic.GetObject(ModularActorVariables_Object.CharacterRigidbody);
        frame = (RobotFrame)logic.GetObject(ModularActorVariables_Object.RobotFrame);
    }

    protected override void OnFixedUpdate(ModularActor actor, ModularActorLogic logic)
    {
        bool grounded = logic.GetBool(ModularActorVariables_Bool.Grounded);
        RobotPieceLegs.MovementSettings currentSettings = grounded ? frame.Legs.GroundedSettings : frame.Legs.AerialSettings;

        float maxSpeed = logic.GetFloat(ModularActorVariables_Float.MaxSpeed);

        Vector2 wasd = new Vector2(logic.GetFloat(ModularActorVariables_Float.Input_Move_X), 
            logic.GetFloat(ModularActorVariables_Float.Input_Move_Y));

        // ----

        Vector3 velocity = rigidbody.velocity;
        float yVelocity = velocity.y;
        velocity.y = 0;

        Vector3 targetVelocity = (rigidbody.transform.right * wasd.x + rigidbody.transform.forward * wasd.y) * maxSpeed;

        float velMagnitude = targetVelocity.sqrMagnitude;
        //accelerate if we are moving, decelerate if we are not.
        float tCoefficient;
        if(velMagnitude > minTurnaroundTargetVelocity * minTurnaroundTargetVelocity
            && rigidbody.velocity.sqrMagnitude > minTurnaroundCurrentVelocity * minTurnaroundCurrentVelocity
            && Vector3.Dot(rigidbody.velocity.normalized, targetVelocity.normalized) <= turnaroundDotAngle)
        {
            tCoefficient = currentSettings.Acceleration * turnaroundAccelerationMultiplier;
            //Debug.Log("turnaround!");
        }
        else if (velMagnitude > rigidbody.velocity.sqrMagnitude)
        {
            tCoefficient = currentSettings.Acceleration;
        }
        else
        {
            tCoefficient = currentSettings.Deceleration;
            float overrideDeceleration = logic.GetFloat(ModularActorVariables_Float.OverrideDeceleration);
            if (overrideDeceleration > tCoefficient)
                tCoefficient = overrideDeceleration;
        }

        velocity = Vector3.MoveTowards(velocity, targetVelocity, tCoefficient * Time.fixedDeltaTime);

        velocity.y = yVelocity;
        rigidbody.velocity = velocity;
    }
}

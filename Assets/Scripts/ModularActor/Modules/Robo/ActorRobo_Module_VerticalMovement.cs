using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRobo_Module_VerticalMovement : BaseActorModule
{

    private new Rigidbody rigidbody;
    private RobotFrame frame;

    private float chargeTime = 0;
    private float floatT = 0;

    private bool prevGrounded;

    [SerializeField]
    private float coyoteTime = 0.05f;

    private float coyoteTimer;

    bool coyoteGrounded;

    protected override void OnInit(ModularActor actor, ModularActorLogic logic)
    {
        rigidbody = (Rigidbody)logic.GetObject(ModularActorVariables_Object.CharacterRigidbody);
        frame = (RobotFrame)logic.GetObject(ModularActorVariables_Object.RobotFrame);
    }

    protected override void OnFixedUpdate(ModularActor actor, ModularActorLogic logic)
    {
        bool targetGrounded = logic.GetBool(ModularActorVariables_Bool.Grounded);
        if(!targetGrounded)
        {
            if (coyoteTimer >= coyoteTime)
                coyoteGrounded = false;
            else
                coyoteTimer += Time.fixedDeltaTime;
        }
        else
        {
            coyoteTimer = 0;
            coyoteGrounded = true;
        }

        bool grounded = coyoteGrounded;

        bool jumpHeld = logic.GetBool(ModularActorVariables_Bool.Input_JumpHeld);
        bool jumpDown = logic.GetBool(ModularActorVariables_Bool.Input_JumpDown);
        bool jumpUp = logic.GetBool(ModularActorVariables_Bool.Input_JumpUp);

        Vector2 wasd = new Vector2(logic.GetFloat(ModularActorVariables_Float.Input_Move_X),
            logic.GetFloat(ModularActorVariables_Float.Input_Move_Y));

        float verticalJumpForce = 0;
        float lateralJumpForce = 0;


        RobotPieceLegs.MovementSettings currentSettings = coyoteGrounded ? frame.Legs.GroundedSettings : frame.Legs.AerialSettings;

        if (prevGrounded != grounded)
        {
            if (currentSettings.TimeUntilMaxCharge == 0)
                chargeTime = 0;
        }

        //Handle charging logic
        if (currentSettings.TimeUntilMaxCharge > 0)
        {
            if (jumpHeld)
            {
                chargeTime += Time.fixedDeltaTime;
                if (chargeTime >= currentSettings.TimeUntilMaxCharge) 
                    chargeTime = currentSettings.TimeUntilMaxCharge;

                Debug.Log(chargeTime);
            }
            if(chargeTime > 0 && jumpUp)
            {
                verticalJumpForce = currentSettings.VerticalImpulseScale
                    * currentSettings.VerticalImpulseOverCharge.Evaluate(chargeTime / currentSettings.TimeUntilMaxCharge);
                lateralJumpForce = currentSettings.LateralImpulseScale
                    * currentSettings.LateralImpulseOverCharge.Evaluate(chargeTime / currentSettings.TimeUntilMaxCharge);
                chargeTime = 0;

                coyoteGrounded = false;
                logic.SetBool(ModularActorVariables_Bool.Grounded, false);
            }
        }
        else // Handle instant jump logic
        {
            if (jumpDown)
            {
                verticalJumpForce = currentSettings.VerticalImpulseScale;
                lateralJumpForce = currentSettings.VerticalImpulseScale;

                coyoteGrounded = false;
                logic.SetBool(ModularActorVariables_Bool.Grounded, false);
            }
        }

        Vector3 velocity = rigidbody.velocity;

        if (verticalJumpForce != 0)
        {
            if (velocity.y < 0) 
                velocity.y = 0;
            velocity += Vector3.up * verticalJumpForce;
        }

        if (lateralJumpForce != 0)
        {
            Vector3 targetVelocity = (rigidbody.transform.right * wasd.x + rigidbody.transform.forward * wasd.y).normalized;
            velocity += targetVelocity * lateralJumpForce;
        }

        //Gravity

        float gravity = frame.Legs.FallGravity;
        if (velocity.y > 0)
        {
            if (!jumpHeld)
                gravity = frame.Legs.JumpNonholdGravity;
            else if (!frame.Legs.DoesCharge)
                gravity = frame.Legs.JumpHoldGravity;
        }

        velocity += Vector3.down * gravity * Time.fixedDeltaTime;
        if (velocity.y < -frame.Legs.MaxFallSpeed) velocity.y = -frame.Legs.MaxFallSpeed;

        if (frame.Legs.CanFloat && !logic.GetBool(ModularActorVariables_Bool.Grounded))
        {
            if (jumpHeld)
            {
                floatT = Mathf.MoveTowards(floatT, 1, Time.fixedDeltaTime * frame.Legs.FloatWindupSpeed);
                if (velocity.y < frame.Legs.FloatUpwardsVelocity)
                    velocity.y = Mathf.Lerp(velocity.y, frame.Legs.FloatUpwardsVelocity, floatT);
            }
            else
            {
                floatT = 0;
            }
        }

        rigidbody.velocity = velocity;

        prevGrounded = grounded;
    }
}

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

    private JumpChargeEffect effect;

    bool coyoteGrounded;

    private JumpState state = JumpState.Stop;

    private float prevJumpForce;

    protected override void OnInit(ModularActor actor, ModularActorLogic logic)
    {
        rigidbody = (Rigidbody)logic.GetObject(ModularActorVariables_Object.CharacterRigidbody);
        frame = (RobotFrame)logic.GetObject(ModularActorVariables_Object.RobotFrame);
        effect = ((RobotPartObject_Legs)frame.GetRobotPartObject(RobotSlot.Legs)).GetJumpChargeEffect();
    }

    protected override void OnFixedUpdate(ModularActor actor, ModularActorLogic logic)
    {
        JumpState newState = JumpState.Stop;

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

                newState = JumpState.Charge;
                if(newState != state)
                {
                    state = newState;
                    if (effect != null) effect.StartCharge();
                }

                if (effect != null)
                {
                    float vertLaunch = currentSettings.VerticalImpulseScale
                        * currentSettings.VerticalImpulseOverCharge.Evaluate(chargeTime / currentSettings.TimeUntilMaxCharge);
                    float latLaunch = currentSettings.LateralImpulseScale
                        * currentSettings.LateralImpulseOverCharge.Evaluate(chargeTime / currentSettings.TimeUntilMaxCharge);

                    float launchIntensity = vertLaunch / currentSettings.VerticalImpulseScale;

                    if(latLaunch > vertLaunch)
                    {
                        launchIntensity = latLaunch / currentSettings.LateralImpulseScale;
                    }

                    effect.SetIntensity(launchIntensity);
                }
            }
            else if(chargeTime > 0 && jumpUp)
            {
                verticalJumpForce = currentSettings.VerticalImpulseScale
                    * currentSettings.VerticalImpulseOverCharge.Evaluate(chargeTime / currentSettings.TimeUntilMaxCharge);
                lateralJumpForce = currentSettings.LateralImpulseScale
                    * currentSettings.LateralImpulseOverCharge.Evaluate(chargeTime / currentSettings.TimeUntilMaxCharge);
                chargeTime = 0;

                coyoteGrounded = false;
                logic.SetBool(ModularActorVariables_Bool.Grounded, false);

                newState = JumpState.Jump;
                if (newState != state)
                {
                    state = newState;
                    if (effect != null) effect.StartJump();
                    prevJumpForce = verticalJumpForce;
                }
            }
        }
        //else // Handle instant jump logic
        //{
        //    if (jumpDown)
        //    {
        //        verticalJumpForce = currentSettings.VerticalImpulseScale;
        //        lateralJumpForce = currentSettings.VerticalImpulseScale;

        //        coyoteGrounded = false;
        //        logic.SetBool(ModularActorVariables_Bool.Grounded, false);
        //    }
        //}

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

        if (state != JumpState.Jump && frame.Legs.CanFloat && !logic.GetBool(ModularActorVariables_Bool.Grounded))
        {
            if (jumpHeld)
            {
                floatT = Mathf.MoveTowards(floatT, 1, Time.fixedDeltaTime * frame.Legs.FloatWindupSpeed);
                if (velocity.y < frame.Legs.FloatUpwardsVelocity)
                    velocity.y = Mathf.Lerp(velocity.y, frame.Legs.FloatUpwardsVelocity, floatT);

                newState = JumpState.Floating;
                if(newState != state)
                {
                    state = newState;
                    if (effect != null) effect.StartHover();
                }

                if (effect != null) effect.SetIntensity(floatT);
            }
            else
            {
                floatT = 0;

                state = JumpState.Stop;
                if (effect != null)
                    effect.Stop();
            }
        }

        rigidbody.velocity = velocity;

        prevGrounded = grounded;

        //Handle jump visual effect
        if(state == JumpState.Jump)
        {
            if (rigidbody.velocity.y <= 0)
            {
                state = JumpState.Stop;
                if (effect != null)
                    effect.Stop();
            }
            else
            {
                if (effect != null)
                    effect.SetIntensity(Mathf.Clamp01(rigidbody.velocity.y / prevJumpForce));
            }
        }

        if(coyoteGrounded && (state == JumpState.Floating || state == JumpState.Jump))
        {
            state = JumpState.Stop;
            if (effect != null)
                effect.Stop();
        }
    }
}

public enum JumpState
{
    Stop,
    Floating,
    Jump,
    Charge
}
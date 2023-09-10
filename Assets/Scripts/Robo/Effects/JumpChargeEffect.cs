using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JumpChargeEffect : MonoBehaviour
{

    protected ModularActorLogic logic;

    protected abstract void Start();

    public void SetLogic(ModularActorLogic logic)
    {
        this.logic = logic;
    }

    public abstract void StartCharge();

    public abstract void StartJump();

    public abstract void StartHover();

    public abstract void Stop();

    public abstract void SetIntensity(float intensity);
}

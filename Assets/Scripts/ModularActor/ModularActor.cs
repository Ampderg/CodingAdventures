using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularActor : MonoBehaviour
{
    [SerializeField]
    private BaseActorModule[] modules;

    public ModularActorVariables Variables = new ModularActorVariables();

    // Start is called before the first frame update
    private void Start()
    {
        //Loop through every module and initialize them.
        for(int i = 0; i < modules.Length; i++)
        {
            modules[i].TryInitialize(this, i);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //Loop through every module and tell them to try to process a graphics frame, if allowed.
        for (int i = 0; i < modules.Length; i++)
        {
            modules[i].TryFrameTick();
        }
    }

    // FixedUpdate is called once per physic tick
    private void FixedUpdate()
    {
        //Loop through every module and tell them to try to process a physics tick, if allowed.
        for (int i = 0; i < modules.Length; i++)
        {
            modules[i].TryPhysicsTick();
        }
    }
}

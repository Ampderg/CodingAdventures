using UnityEngine;

[System.Serializable]
public abstract class BaseActorModule
{
    //Keeps track of whether or not this module has been initialized or not, and what the current module ID is.
    protected int moduleId = -1;
    //Keeps track of the ModularActor that initialized this module. Used for checking conditions and applying changes during logic.
    protected ModularActor parentActor;


    /// <summary>
    /// Called during Start(). Attempts to tell the module to process a physics tick.
    /// </summary>
    public void TryInitialize(ModularActor sender, int moduleId)
    {
        //Check to see if this module has already been initialized, and throw a warning and return if it has.
        if(this.moduleId < 0)
        {
            Debug.LogWarning($"{this.ToString()} attempted to be initialized by {sender.gameObject.name}, despite already being initialized by {parentActor.gameObject.name}!");
            return;
        }
        this.moduleId = moduleId;

        //Assign the parent actor for future referencing.
        this.parentActor = sender;

        //Call the initialization method on the inherited module.
        OnInitialize();
    }

    /// <summary>
    /// Called during Update(). Attempts to tell the module to process a graphics tick.
    /// </summary>
    public void TryFrameTick()
    {
        if (this.CanFrameTick())
            OnFrameTick();
    }

    /// <summary>
    /// Called during FixedUpdate(). Attempts to tell the module to process a physics tick.
    /// </summary>
    public void TryPhysicsTick()
    {
        if (this.CanPhysicsTick())
            OnPhysicsTick();
    }

    /// <summary>
    /// Called during Update()
    /// </summary>
    /// <returns>Returns true if the module should be allowed to tick</returns>
    protected virtual bool CanFrameTick() => true;
    /// <summary>
    /// Called during FixedUpdate()
    /// </summary>
    /// <returns>Returns true if the module should be allowed to tick</returns>
    protected virtual bool CanPhysicsTick() => true;

    protected abstract void OnInitialize();
    protected abstract void OnFrameTick();
    protected abstract void OnPhysicsTick();

}

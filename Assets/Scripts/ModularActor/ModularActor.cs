using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularActor : MonoBehaviour
{
    public enum TickPhase
    {
        Update,
        FixedUpdate,
        ProcessInput,
        Init
    }

    [System.Serializable]
    public class ModuleDictionary : SerializableDictionary<ActorModuleType, BaseActorModule> { }

    [SerializeField]
    private ModuleDictionary modules;
    [SerializeField]
    private BaseActorModule[] statelessModules;
    [SerializeField]
    private ModularActorLogic logic;

    // Start is called before the first frame update
    void Start()
    {
        if (logic != null)
            SetLogic(logic);

        logic.TryInit();
    }

    // Update is called once per frame
    void Update()
    {
        logic.TryUpdate();
    }

    private void FixedUpdate()
    {
        logic.TryFixedUpdate();
    }

    public void ProcessTick(ActorModuleType moduleType, TickPhase phase)
    {
        if (moduleType == ActorModuleType.AlwaysTick)
        {
            foreach(BaseActorModule m in statelessModules)
            {
                m.ProcessTick(phase, this, logic);
            }
        }
        else
        {
            if (!modules.ContainsKey(moduleType))
            {
                return;
            }

            modules[moduleType].ProcessTick(phase, this, logic);
        }
    }

    private void SetLogic(ModularActorLogic newLogic)
    {
        if (this.logic != null)
            this.logic.Dispose();

        this.logic = newLogic;

        this.logic.SetParent(this);
    }
}

public class BaseActorModule : MonoBehaviour
{
    public void ProcessTick(ModularActor.TickPhase phase, ModularActor actor, ModularActorLogic logic)
    {
        if (!this.gameObject.activeInHierarchy)
            return;

        switch (phase)
        {
            case ModularActor.TickPhase.Init:
                OnInit(actor, logic);
                break;
            case ModularActor.TickPhase.Update:
                OnUpdate(actor, logic);
                break;
            case ModularActor.TickPhase.FixedUpdate:
                OnFixedUpdate(actor, logic);
                break;
        }
    }
    protected virtual void OnInit(ModularActor actor, ModularActorLogic logic) { }
    protected virtual void OnUpdate(ModularActor actor, ModularActorLogic logic) { }
    protected virtual void OnFixedUpdate(ModularActor actor, ModularActorLogic logic) { }
}

public class ModularActorLogic : MonoBehaviour
{
    protected ModularActor parent;

    public virtual void TryInit()
    {
        parent.ProcessTick(ActorModuleType.AlwaysTick, ModularActor.TickPhase.Init);
    }

    public virtual void TryUpdate()
    {
        this.ProcessInput();
        parent.ProcessTick(ActorModuleType.AlwaysTick, ModularActor.TickPhase.Update);
    }

    public virtual void TryFixedUpdate()
    {
        parent.ProcessTick(ActorModuleType.AlwaysTick, ModularActor.TickPhase.FixedUpdate);
    }

    protected virtual void ProcessInput() { }

    public virtual void Dispose() { }

    public void SetParent(ModularActor newParent)
    {
        this.parent = newParent;
    }

    #region Variables

    private Dictionary<ModularActorVariables_Float, float> variableFloats;

    public void SetFloat(ModularActorVariables_Float variable, float value)
    {
        if (variableFloats == null) variableFloats = new Dictionary<ModularActorVariables_Float, float>();
        variableFloats[variable] = value;
    }
    public float GetFloat(ModularActorVariables_Float variable)
    {
        if (variableFloats == null || !variableFloats.ContainsKey(variable)) return 0;
        return variableFloats[variable];
    }


    private Dictionary<ModularActorVariables_Bool, bool> variableBools;

    public void SetBool(ModularActorVariables_Bool variable, bool value)
    {
        if (variableBools == null) variableBools = new Dictionary<ModularActorVariables_Bool, bool>();
        variableBools[variable] = value;
    }
    public bool GetBool(ModularActorVariables_Bool variable)
    {
        if (variableBools == null || !variableBools.ContainsKey(variable)) return false;
        return variableBools[variable];
    }

    private Dictionary<ModularActorVariables_Object, object> variableObject;

    public void SetObject(ModularActorVariables_Object variable, object value)
    {
        if (variableObject == null) variableObject = new Dictionary<ModularActorVariables_Object, object>();
        variableObject[variable] = value;
    }
    public object GetObject(ModularActorVariables_Object variable)
    {
        if (variableObject == null || !variableObject.ContainsKey(variable)) return null;
        return variableObject[variable];
    }

    #endregion
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ConditionalActorModule : BaseActorModule
{
    [SerializeField]
    private ActorModuleCondition[] conditions;

    protected override bool CanFrameTick()
    {
        for(int i = 0; i < conditions.Length; i++)
        {
            if (!conditions[i].IsTrue())
                return false;
        }
        return true;
    }
}

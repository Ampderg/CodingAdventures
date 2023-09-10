using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotPartObject_Legs : RobotPartObject
{
    [SerializeField]
    private JumpChargeEffect effect;

    public JumpChargeEffect GetJumpChargeEffect()
    {
        return effect;
    }
}
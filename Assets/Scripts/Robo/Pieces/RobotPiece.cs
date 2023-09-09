using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RobotPiece : ScriptableObject
{
    public abstract RobotSlot GetSlotType();
}

public enum RobotSlot
{
    Legs,
    Arms
}

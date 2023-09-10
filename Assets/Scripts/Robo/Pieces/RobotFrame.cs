using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotFrame : MonoBehaviour
{

    public RobotPieceLegs Legs;

    [SerializeField]
    private Transform partObjectsInstantiateRoot;

    private Dictionary<RobotSlot, RobotPartObject> partObjects;

    public RobotPartObject GetRobotPartObject(RobotSlot type)
    {
        if (!partObjects.ContainsKey(type)) return null;

        return partObjects[type];
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void BuildRobotMesh()
    {
        partObjects = new Dictionary<RobotSlot, RobotPartObject>();
        partObjects[RobotSlot.Legs] = Instantiate(Legs.Model.gameObject, partObjectsInstantiateRoot).GetComponent<RobotPartObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

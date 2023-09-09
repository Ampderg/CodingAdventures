using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRobo_Module_CameraOffset : BaseActorModule
{
    [SerializeField]
    private Rigidbody referenceRigidbody;
    [SerializeField]
    private Transform cameraOffsetTransform;
    [SerializeField]
    private float horizontalLagRatio = 0.5f;
    [SerializeField]
    private float verticalLagRatio = 0.5f;
    [SerializeField]
    private float forwardLagRatio = 0.5f;
    [SerializeField]
    private float dampingTime = 1;

    private Vector3 currentTargetPos;

    protected override void OnFixedUpdate(ModularActor actor, ModularActorLogic logic)
    {
        float verticalFactor = Vector3.Dot(cameraOffsetTransform.forward, Vector3.up);


        Vector3[] projectedVectors = new Vector3[]
        {
            Vector3.Project(referenceRigidbody.velocity, referenceRigidbody.transform.right),
            Vector3.Project(referenceRigidbody.velocity, referenceRigidbody.transform.forward),
            Vector3.Project(referenceRigidbody.velocity, Vector3.up)
        };

        Vector3 targetPos = Vector3.zero;

        float forwardValue = projectedVectors[1].magnitude * -Mathf.Sign(Vector3.Dot(projectedVectors[1], referenceRigidbody.transform.forward)) * forwardLagRatio;
        float verticalValue = projectedVectors[2].magnitude * -Mathf.Sign(Vector3.Dot(projectedVectors[2], Vector3.up)) * verticalLagRatio;

        targetPos.x = projectedVectors[0].magnitude * -Mathf.Sign(Vector3.Dot(projectedVectors[0], referenceRigidbody.transform.right)) * horizontalLagRatio;
        targetPos.z = Mathf.Lerp(forwardValue, verticalValue * Mathf.Sign(verticalFactor), Mathf.Abs(verticalFactor));
        targetPos.y = Mathf.Lerp(verticalValue, forwardValue, Mathf.Abs(verticalFactor));

        currentTargetPos = Vector3.MoveTowards(currentTargetPos, targetPos, 1);
        Vector3 dampenedPos = Vector3.Lerp(cameraOffsetTransform.localPosition, currentTargetPos, Time.fixedDeltaTime * dampingTime);

        cameraOffsetTransform.transform.localPosition = dampenedPos;
    }
}

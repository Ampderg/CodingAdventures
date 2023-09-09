using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorRobo_Module_CameraRotation : BaseActorModule
{
    [SerializeField]
    private Transform characterRotationTarget;
    [SerializeField]
    private Transform characterPosition;
    [SerializeField]
    private new Camera camera;
    [SerializeField]
    private Transform cameraTransform;
    
    [SerializeField]
    private Transform cameraTarget;
    [SerializeField]
    private float rotationSpeed = 1;
    [SerializeField]
    private float minVerticalRotation = 5f;
    [SerializeField]
    private float maxVerticalRotation = 170f;
    [SerializeField]
    private float dampingCoefficient = 1f;
    [SerializeField]
    private float minCameraDistance = 1f;
    [SerializeField]
    private float maxCameraDistance;
    [SerializeField]
    private LayerMask cameraCollisionMask;

    private Vector3 localRotation = Vector3.zero;
    private Vector3 targetRotation = Vector3.zero;



    protected override void OnUpdate(ModularActor actor, ModularActorLogic logic)
    {
        Vector2 rotation = new Vector2(logic.GetFloat(ModularActorVariables_Float.Input_Camera_X),
            logic.GetFloat(ModularActorVariables_Float.Input_Camera_Y));

        // ----
        targetRotation.x += rotation.x * rotationSpeed;
        targetRotation.y += rotation.y * rotationSpeed;
        targetRotation.y = Mathf.Clamp(targetRotation.y, minVerticalRotation, maxVerticalRotation);

        localRotation = Vector3.Lerp(localRotation, targetRotation, Time.deltaTime * dampingCoefficient);

        Quaternion qRot = Quaternion.Euler(localRotation.y, localRotation.x, 0f);
        Vector3 lookVector = qRot * Vector3.forward;
        cameraTransform.rotation = qRot;

        float adjustedCameraDistance = maxCameraDistance;
        RaycastHit hit;

        Vector3[] cameraRelativePoints = new Vector3[]
        {
            camera.ViewportToWorldPoint(new Vector3(0, 0, camera.nearClipPlane)) - cameraTransform.position,
            camera.ViewportToWorldPoint(new Vector3(0, 1, camera.nearClipPlane)) - cameraTransform.position,
            camera.ViewportToWorldPoint(new Vector3(1, 0, camera.nearClipPlane)) - cameraTransform.position,
            camera.ViewportToWorldPoint(new Vector3(1, 1, camera.nearClipPlane)) - cameraTransform.position
        };

        foreach (Vector3 cameraPoint in cameraRelativePoints)
        {
            if (Physics.Raycast(cameraTarget.position + cameraPoint, -lookVector, out hit, maxCameraDistance, cameraCollisionMask))
                adjustedCameraDistance = Mathf.Clamp(hit.distance, minCameraDistance, adjustedCameraDistance);
        }

        cameraTransform.position = cameraTarget.position + (-lookVector * adjustedCameraDistance);

        characterRotationTarget.position = characterPosition.position + new Vector3(lookVector.x, 0, lookVector.z).normalized;
    }
}

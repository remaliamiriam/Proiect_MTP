using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 6f;
    public float height = 3f;
    public float followSpeed = 10f;
    public float wallOffset = 0.2f; 
    public LayerMask obstructionLayers; 

    private Vector3 currentVelocity;

    void LateUpdate()
    {
        if (!target) return;

       
        Vector3 desiredOffset = -target.forward * distance + Vector3.up * height;
        Vector3 desiredPosition = target.position + desiredOffset;

       
        Vector3 directionToCamera = desiredOffset.normalized;
        float maxDistance = desiredOffset.magnitude;

        RaycastHit hit;
        Vector3 finalPosition = desiredPosition;

        if (Physics.Raycast(target.position + Vector3.up * 1.5f, directionToCamera, out hit, maxDistance, obstructionLayers))
        {
            
            finalPosition = hit.point - directionToCamera * wallOffset;
        }


        transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref currentVelocity, 1f / followSpeed);

       
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}

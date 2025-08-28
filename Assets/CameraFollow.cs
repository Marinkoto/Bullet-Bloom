using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;           // Tank transform
    public float distance = 10f;       // How far behind the tank
    public float height = 5f;          // How high above the tank
    public float followSpeed = 10f;    // How quickly camera moves to position
    public float rotationSpeed = 5f;   // How quickly camera rotates to match tank facing

    private void LateUpdate()
    {
        if (target == null) return;

        // Desired position: behind the tank based on tank's forward direction
        Vector3 desiredPosition = target.position
                                  - target.forward * distance
                                  + Vector3.up * height;

        // Smoothly move the camera
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Smoothly rotate the camera to look at the tank
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

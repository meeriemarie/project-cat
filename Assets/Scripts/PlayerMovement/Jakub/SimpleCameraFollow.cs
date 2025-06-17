using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target; // the object to follow (your capsule)
    public float mouseSensitivity = 2f;

    private float rotationY = 0f;

    void LateUpdate()
    {
        rotationY += Input.GetAxis("Mouse X") * mouseSensitivity;

        // Keep the camera pivot at the target's position
        transform.position = target.position;

        // Rotate around Y axis based on mouse input
        transform.rotation = Quaternion.Euler(20, rotationY, 0);
    }
}

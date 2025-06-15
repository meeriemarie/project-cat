using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 6f;
    public float height = 3f;
    public float mouseSensitivity = 3f;
    public float rotationSmoothTime = 0.1f;

    private float _yaw;
    private float _pitch;
    private Vector3 _currentRotation;
    private Vector3 _rotationSmoothVelocity;

    public float minPitch = -30f;
    public float maxPitch = 60f;

    void LateUpdate()
    {
        if (target == null) return;

        // Mouse input
        _yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        _pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

        // Smooth the rotation
        Vector3 targetRotation = new Vector3(_pitch, _yaw);
        _currentRotation = Vector3.SmoothDamp(_currentRotation, targetRotation, ref _rotationSmoothVelocity, rotationSmoothTime);

        // Apply rotation and position
        transform.eulerAngles = _currentRotation;

        Vector3 offset = new Vector3(0, height, -distance);
        transform.position = target.position + transform.rotation * offset;
    }
}

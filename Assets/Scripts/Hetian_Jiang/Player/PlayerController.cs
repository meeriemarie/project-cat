using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 5f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 5f;

    [Header("Dash")]
    [SerializeField] private float _dashForce = 10f;
    [SerializeField] private float _dashCooldown = 1f;

    private bool _canDash = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundLayer;
    public LayerMask objectLayer;

    [Header("Camera")]
    public Transform cameraTransform;

    // Private state
    private Rigidbody _rb;
    private Vector2 _moveInput;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // Prevent flopping: only allow Y-axis rotation
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        // Check if the player is grounded
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Optional: apply slight damping to horizontal velocity in air
        if (!_isGrounded)
        {
            Vector3 vel = _rb.velocity;
            vel.x *= 0.98f;
            vel.z *= 0.98f;
            _rb.velocity = vel;
        }
    }

    private void FixedUpdate()
    {
        if (cameraTransform == null) return;

        // Get camera's flattened forward/right directions
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Compute movement direction based on input and camera
        Vector3 move = forward * _moveInput.y + right * _moveInput.x;

        // Move player
        _rb.MovePosition(_rb.position + _moveSpeed * Time.fixedDeltaTime * move);

        // Rotate to face movement direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            _rb.rotation = Quaternion.Slerp(_rb.rotation, targetRotation, _rotationSpeed * Time.fixedDeltaTime);
        }
    }

    // Called by Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump()
    {
        if (_isGrounded)
        {
            // Reset Y velocity to avoid buildup
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    public void OnDash()
    {
        if (!_canDash || !_isGrounded) return;

        // Get dash direction from input
        Vector3 dashDir = (cameraTransform.forward * _moveInput.y + cameraTransform.right * _moveInput.x).normalized;
        dashDir.y = 0;

        // Default to forward if no input
        if (dashDir == Vector3.zero)
            dashDir = transform.forward;

        // Cancel current velocity and apply dash
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dashDir * _dashForce, ForceMode.Impulse);

        // Start cooldowns
        StartCoroutine(EndDashEarly());
        StartCoroutine(DashCooldownRoutine());
    }

    private IEnumerator EndDashEarly()
    {
        yield return new WaitForSeconds(0.5f);
        // Stop horizontal velocity but preserve vertical
        _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
    }

    private IEnumerator DashCooldownRoutine()
    {
        _canDash = false;
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }

    // Shows ground check sphere in editor
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
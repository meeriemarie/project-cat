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

    [Header("Camera")]
    public Transform cameraTransform;

    [Header("Animator")]
    [SerializeField] private Animator _animator;

    private Rigidbody _rb;
    private Vector2 _moveInput;
    private bool _isGrounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // Safety check
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Update animator parameters
        bool isWalking = _moveInput.magnitude > 0.1f;
        _animator.SetBool("isWalking", isWalking);
        _animator.SetBool("isGrounded", _isGrounded);
    }

    private void FixedUpdate()
    {
        if (cameraTransform == null) return;

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * _moveInput.y + right * _moveInput.x;
        _rb.MovePosition(_rb.position + _moveSpeed * Time.fixedDeltaTime * move);

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
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            _animator.SetTrigger("jumpTrigger");
        }
    }

    public void OnDash()
    {
        if (!_canDash || !_isGrounded) return;

        Vector3 dashDir = (cameraTransform.forward * _moveInput.y + cameraTransform.right * _moveInput.x).normalized;
        dashDir.y = 0;
        if (dashDir == Vector3.zero) dashDir = transform.forward;

        _rb.velocity = Vector3.zero;
        _rb.AddForce(dashDir * _dashForce, ForceMode.Impulse);

        _animator.speed = 3f; // Triple animation speed during dash

        StartCoroutine(EndDashEarly());
        StartCoroutine(DashCooldownRoutine());
    }

    public void OnScratch()
    {
        _animator.SetTrigger("scratchTrigger");
    }

    private IEnumerator EndDashEarly()
    {
        yield return new WaitForSeconds(0.5f);
        _rb.velocity = new Vector3(0f, _rb.velocity.y, 0f);
        _animator.speed = 1f; // Reset to normal speed
    }

    private IEnumerator DashCooldownRoutine()
    {
        _canDash = false;
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}

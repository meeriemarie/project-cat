using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DogChaser : ChaserBase
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float dashForce = 12f;
    public float dashCooldown = 2f;
    public float dashRange = 5f;

    [Header("Detection Settings")]
    public float lostSightDelay = 3f;
    public LayerMask obstacleMask;

    [Header("Capture Settings")]
    public float captureDistance = 2f;
    public float captureDuration = 3f;

    [Header("Patrol Settings")]
    public float patrolRadius = 10f;
    public float patrolWaitTime = 1f;

    [Header("Animator")]
    [SerializeField] private Animator _animator;

    private NavMeshAgent _agent;
    private Rigidbody _rb;
    private Coroutine _captureCoroutine;
    private bool _isChasing = false;
    private float _timeSinceLastSeen = 0f;
    private bool _canDash = true;
    private bool _isDashing = false;

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();

        _agent.speed = moveSpeed;
        _agent.stoppingDistance = 0f;
        _agent.autoBraking = true;

        _rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (_isDashing) return;

        bool canSee = CanSeePlayer();

        if (canSee)
        {
            _isChasing = true;
            _timeSinceLastSeen = 0f;
            Chase();
        }
        else if (_isChasing)
        {
            _timeSinceLastSeen += Time.deltaTime;
            if (_timeSinceLastSeen >= lostSightDelay)
                _isChasing = false;
        }

        if (!_isChasing)
        {
            Patrol();
        }


        if (_agent.velocity.magnitude > 0.1f)
            _animator.SetBool("isWalking", true);
        else
            _animator.SetBool("isWalking", false);
    }

    protected override void Chase()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        _agent.SetDestination(player.position);

        if (_canDash && distance < dashRange)
        {
            StartCoroutine(Dash((player.position - transform.position).normalized));
        }

        if (distance < captureDistance)
        {
            if (_captureCoroutine == null)
                _captureCoroutine = StartCoroutine(CapturePlayer());
        }
        else
        {
            if (_captureCoroutine != null)
            {
                StopCoroutine(_captureCoroutine);
                _captureCoroutine = null;
            }
        }
    }

    private IEnumerator Dash(Vector3 dir)
    {
        _isDashing = true;
        _canDash = false;

        // Disable NavMeshAgent
        _agent.enabled = false;

        // Apply dash force
        _rb.velocity = Vector3.zero;
        _rb.AddForce(dir.normalized * dashForce, ForceMode.Impulse);

        _animator.speed = 3f; // Triple animation speed during dash

        // Wait for dash duration
        yield return new WaitForSeconds(0.5f);

        // Stop Rigidbody and re-enable NavMeshAgent
        _rb.velocity = Vector3.zero;
        _agent.enabled = true;

        _isDashing = false;
        _animator.speed = 1f; // Reset to normal speed

        yield return new WaitForSeconds(dashCooldown);
        _canDash = true;
    }

    private void Patrol()
    {
        if (!_agent.hasPath || _agent.remainingDistance < 0.5f)
        {
            Vector3 randomPoint;
            if (GetRandomPoint(transform.position, patrolRadius, out randomPoint))
            {
                _agent.SetDestination(randomPoint);
            }
        }
    }

    private bool GetRandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPos = center + Random.insideUnitSphere * range;
            randomPos.y = center.y;
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = center;
        return false;
    }

    private IEnumerator CapturePlayer()
    {
        float timer = 0f;

        while (Vector3.Distance(transform.position, player.position) < captureDistance)
        {
            timer += Time.deltaTime;

            if (timer >= captureDuration)
            {
                Debug.Log("Game Over: The cat has been caught by the dog!");
                GameManager.Instance.OnPlayerCaught();
                break;
            }

            yield return null;
        }

        _captureCoroutine = null;
    }

    protected override bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 dirToPlayer = player.position - transform.position;
        float distance = dirToPlayer.magnitude;

        Debug.DrawLine(transform.position + Vector3.up, player.position + Vector3.up, Color.red);

        if (distance <= detectionRadius)
        {
            if (!Physics.Raycast(transform.position + Vector3.up, dirToPlayer.normalized, distance, obstacleMask))
            {
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, dashRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, captureDistance);
    }
}

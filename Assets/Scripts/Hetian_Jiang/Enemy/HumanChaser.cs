using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanChaser : ChaserBase
{
    [Header("Vision Settings")]
    public float viewAngle = 90f;
    public float viewDistance = 15f;

    [Tooltip("How many seconds the human chases after losing sight.")]
    public float lostSightDelay = 3f;

    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    private int _currentPatrolIndex = 0;

    [Header("Capture Settings")]
    public float captureDistance = 2f;
    public float captureDuration = 3f;

    private NavMeshAgent _agent;
    private Coroutine _captureCoroutine;

    private bool _isChasing = false;
    private float _timeSinceLastSeen = 0f;

    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        DrawVisionCone();

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
            {
                _isChasing = false;
            }
        }

        if (!_isChasing)
        {
            Patrol();
        }
    }

    protected override void Chase()
    {
        if (player == null) return;

        _agent.SetDestination(player.position);

        if (Vector3.Distance(transform.position, player.position) < captureDistance)
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

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % patrolPoints.Length;
            _agent.SetDestination(patrolPoints[_currentPatrolIndex].position);
        }
    }

    private IEnumerator CapturePlayer()
    {
        float timer = 0f;

        while (Vector3.Distance(transform.position, player.position) < captureDistance)
        {
            timer += Time.deltaTime;

            if (timer >= captureDuration)
            {
                Debug.Log("Game Over: The cat has been caught!");
                GameManager.Instance.OnPlayerCaught();
                break;
            }

            yield return null;
        }

        _captureCoroutine = null;
    }

    protected override bool CanSeePlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("HumanChaser: Player reference is null.");
            return false;
        }

        // Adjust for scale: Human's eye height & Player's mid-body
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 target = player.position + Vector3.up * 0.1f;

        Vector3 dirToPlayer = (target - origin).normalized;
        float distanceToPlayer = Vector3.Distance(origin, target);
        float angleToPlayer = Vector3.Angle(transform.forward, dirToPlayer);

        if (distanceToPlayer <= viewDistance && angleToPlayer <= viewAngle / 2f)
        {
            Debug.DrawLine(origin, target, Color.yellow); // visualize line of sight

            if (Physics.Raycast(origin, dirToPlayer, out RaycastHit hit, distanceToPlayer, ~0))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        return false;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2f, false);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2f, false);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewDistance);
    }

    private Vector3 DirFromAngle(float angleInDegrees, bool global)
    {
        if (!global)
            angleInDegrees += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private void DrawVisionCone()
    {
        Vector3 origin = transform.position + Vector3.up;
        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2, false);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2, false);

        Debug.DrawLine(origin, origin + leftBoundary * viewDistance, Color.green);
        Debug.DrawLine(origin, origin + rightBoundary * viewDistance, Color.green);
    }
}

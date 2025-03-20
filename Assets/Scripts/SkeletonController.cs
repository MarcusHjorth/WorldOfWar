using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour
{
    public enum SkeletonState
    {
        Patrolling,
        Chasing,
        Attacking,
        Returning
    }

    [Header("Waypoint Following")] public List<Transform> waypoints;
    public float waypointStoppingDistance = 0.1f;

    [Header("Player Detection")] public float detectionRadius = 10f;
    public float chaseRadius = 15f;
    public float attackDistance = 2f;
    public LayerMask playerLayer;

    [Header("Movement")] public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5f;

    [Header("Attack")] public float attackCooldown = 2f;
    public int attackDamage = 10;

    private NavMeshAgent _agent;
    private Animator _animator;
    private int _currentWaypointIndex = 0;
    private SkeletonState _currentState = SkeletonState.Patrolling;
    private Transform _detectedPlayer;
    private float _lastAttackTime;
    private Health _health;
    private Collider _collider;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();
        _collider = GetComponent<Collider>();

        _agent.speed = patrolSpeed;
        UpdateAnimatorSpeed();

        if (waypoints.Count > 0)
        {
            SetDestination(waypoints[_currentWaypointIndex].position, waypointStoppingDistance);
        }

        _health.OnDamage += OnDamage;
        _health.OnDie += OnDie;
    }

    void Update()
    {
        if (_health.IsDead()) return;

        switch (_currentState)
        {
            case SkeletonState.Patrolling:
                Patrol();
                DetectPlayer();
                break;
            case SkeletonState.Chasing:
                ChasePlayer();
                break;
            case SkeletonState.Attacking:
                Attack();
                break;
            case SkeletonState.Returning:
                ReturnToPatrol();
                break;
        }

        UpdateAnimatorSpeed();
    }

    void Patrol()
    {
        if (waypoints.Count == 0) return;

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Count;
            SetDestination(waypoints[_currentWaypointIndex].position, waypointStoppingDistance);
        }
    }

    void DetectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                _detectedPlayer = hit.transform;
                SetState(SkeletonState.Chasing);
                break;
            }
        }
    }

    void ChasePlayer()
    {
        if (_detectedPlayer == null)
        {
            SetState(SkeletonState.Returning);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _detectedPlayer.position);
        if (distanceToPlayer <= attackDistance)
        {
            SetState(SkeletonState.Attacking);
        }
        else if (distanceToPlayer <= chaseRadius)
        {
            SetDestination(_detectedPlayer.position, attackDistance);
        }
        else
        {
            _detectedPlayer = null;
            SetState(SkeletonState.Returning);
        }
    }

    void Attack()
    {
        if (_detectedPlayer == null)
        {
            SetState(SkeletonState.Returning);
            return;
        }

        _agent.SetDestination(transform.position); // Stop moving

        if (Time.time - _lastAttackTime >= attackCooldown)
        {
            // Trigger attack animation
            _animator.SetTrigger("Attack");
            _lastAttackTime = Time.time;
        }

        // Check if the player is out of range and transition back to chasing
        float distanceToPlayer = Vector3.Distance(transform.position, _detectedPlayer.position);
        if (distanceToPlayer > attackDistance)
        {
            SetState(SkeletonState.Chasing);
        }
    }

    void ReturnToPatrol()
    {
        if (waypoints.Count == 0)
        {
            SetState(SkeletonState.Patrolling);
            return;
        }

        int nearestWaypointIndex = GetNearestWaypointIndex();
        SetDestination(waypoints[nearestWaypointIndex].position, waypointStoppingDistance);

        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _currentWaypointIndex = nearestWaypointIndex;
            SetState(SkeletonState.Patrolling);
        }
    }

    int GetNearestWaypointIndex()
    {
        int nearestIndex = 0;
        float nearestDistance = float.MaxValue;

        for (int i = 0; i < waypoints.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i].position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIndex = i;
            }
        }

        return nearestIndex;
    }

    void SetDestination(Vector3 destination, float stoppingDistance)
    {
        _agent.stoppingDistance = stoppingDistance;
        _agent.SetDestination(destination);
    }

    void SetState(SkeletonState newState)
    {
        if (_currentState != newState)
        {
            _currentState = newState;
            UpdateAgentSpeed();

            // Reset attack trigger when leaving attack state
            if (_currentState != SkeletonState.Attacking)
            {
                _animator.ResetTrigger("Attack");
            }
        }
    }

    void UpdateAgentSpeed()
    {
        _agent.speed = (_currentState == SkeletonState.Chasing) ? chaseSpeed : patrolSpeed;
    }

    void UpdateAnimatorSpeed()
    {
        float normalizedSpeed = Mathf.InverseLerp(0, chaseSpeed, _agent.velocity.magnitude);

        _animator.SetFloat("Speed", normalizedSpeed);
    }

    // This method will be called by the animation event during the attack animation
    public void OnAttackHit()
    {
        if (_detectedPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, _detectedPlayer.position);

            if (distanceToPlayer <= attackDistance && _detectedPlayer.TryGetComponent(out Health health))
            {
                Debug.Log("Damage player!");
                health.Damage(attackDamage);
            }
        }
    }

    void OnDamage()
    {
        _animator.SetTrigger("Damage");
    }

    void OnDie()
    {
        _animator.SetTrigger("Die");
        _agent.isStopped = true;
        
        _collider.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
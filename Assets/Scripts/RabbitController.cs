using UnityEngine;
using UnityEngine.AI;

public class RabbitController : MonoBehaviour
{
    public enum RabbitState
    {
        Exploring,
        Fleeing,
    }

    [Header("Waypoint Following")] public float waypointStoppingDistance = 0.1f;

    [Header("Player Detection")] public float detectionRadius = 10f;
    public float fleeRadius = 20f;
    public LayerMask playerLayer;

    [Header("Movement")] public float exploreSpeed = 3.5f;
    public float fleeSpeed = 5f;

    private NavMeshAgent _agent;
    private Animator _animator;
    private RabbitState _currentState = RabbitState.Exploring;
    private Transform _detectedPlayer;
    private Health _health;

    private Vector3 _waypointPosition;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _health = GetComponent<Health>();

        _agent.speed = exploreSpeed;
        UpdateAnimatorSpeed();

        _health.OnDie += OnDie;
    }

    void Update()
    {
        if (_health.IsDead()) return;

        switch (_currentState)
        {
            case RabbitState.Exploring:
                Explore();
                DetectPlayer();
                break;
            case RabbitState.Fleeing:
                Flee();
                break;
        }

        UpdateAnimatorSpeed();
    }

    void Explore()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (Random.value < 0.25f)
            {
                SetDestination(CreateNewExploreWaypoint(), waypointStoppingDistance);
            }
        }
    }

    Vector3 CreateNewExploreWaypoint()
    {
        Vector3 direction = Vector3.Lerp(transform.forward, Random.insideUnitSphere, 0.6f);

        return transform.position + direction * 15f;
    }

    void Flee()
    {
        if (_detectedPlayer == null)
        {
            SetState(RabbitState.Exploring);
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _detectedPlayer.position);
        if (distanceToPlayer <= fleeRadius)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (Random.value < 0.75f)
                {
                    SetDestination(CreateNewFleeWaypoint(), waypointStoppingDistance);
                }
            }
        }
        else
        {
            _detectedPlayer = null;
            SetState(RabbitState.Exploring);
        }
    }

    Vector3 CreateNewFleeWaypoint()
    {
        int flip = Random.value < 0.2 ? -1 : 1;
        Vector3 toPlayer = transform.position - _detectedPlayer.position;
        Vector3 direction = Vector3.Lerp(transform.forward, toPlayer.normalized * flip, 0.8f);
        Vector3 turnDirection = transform.right * Random.Range(-8f, 8f);

        return transform.position + direction * 15f + turnDirection;
    }

    void DetectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player"))
            {
                _detectedPlayer = hit.transform;
                SetState(RabbitState.Fleeing);
                break;
            }
        }
    }

    void SetDestination(Vector3 destination, float stoppingDistance)
    {
        _agent.stoppingDistance = stoppingDistance;
        _agent.SetDestination(destination);
    }

    void SetState(RabbitState newState)
    {
        if (_currentState != newState)
        {
            if (_currentState == RabbitState.Fleeing)
            {
                _agent.ResetPath();
            }

            _currentState = newState;
            UpdateAgentSpeed();
        }
    }

    void UpdateAgentSpeed()
    {
        _agent.speed = (_currentState == RabbitState.Fleeing) ? fleeSpeed : exploreSpeed;
    }

    void UpdateAnimatorSpeed()
    {
        float normalizedSpeed = Mathf.InverseLerp(0, fleeSpeed, _agent.velocity.magnitude);

        _animator.SetFloat("Speed", normalizedSpeed);
    }

    void OnDie()
    {
        _animator.SetTrigger("Die");
        _agent.isStopped = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fleeRadius);
    }
}
using UnityEngine;
using UnityEngine.AI;

public class RabbitController : MonoBehaviour
{
    public enum RabbitState
    {
        Exploring,
        Fleeing,
    }

    [Header("Waypoint Following")] public float waypointStoppingDistance = 3f;

    [Header("Home Settings")] public Transform homeTransform;
    public float maxDistanceFromHome = 30f;
    public float homeGravityStrength = 0.3f;

    [Header("Player Detection")] public float detectionRadius = 10f;
    public float fleeRadius = 25f;
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
        Vector3 currentPosition = transform.position;

        // Calculate a random direction
        Vector3 randomDirection = Random.insideUnitSphere;

        // Lerp between forward direction and random direction
        Vector3 explorationDirection = Vector3.Lerp(transform.forward, randomDirection, 0.6f);

        if (homeTransform != null)
        {
            Vector3 homePosition = homeTransform.position; // Get the position of the home Transform
            Vector3 directionToHome = homePosition - currentPosition;
            float distanceFromHome = directionToHome.magnitude;

            // If the rabbit is far from home, add a component pointing towards home
            if (distanceFromHome > maxDistanceFromHome)
            {
                explorationDirection =
                    Vector3.Lerp(explorationDirection, directionToHome.normalized, homeGravityStrength);
            }
        }

        // Normalize the direction, scale it and add it to the currentPosition
        return currentPosition + explorationDirection.normalized * 15f;
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
        Vector3 toPlayer = transform.position - _detectedPlayer.position;
        Vector3 fleeDirection = toPlayer.normalized;

        // Create a rotation to randomly deviate from the direct flee direction
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(-110f, 110f), 0);
        fleeDirection = randomRotation * fleeDirection;

        // Occasionally allow for more drastic direction changes
        if (Random.value < 0.2f)
        {
            fleeDirection = Random.onUnitSphere;
            fleeDirection.y = 0; // Keep it on the horizontal plane
            fleeDirection = fleeDirection.normalized;
        }

        // Slightly bias towards moving forward
        fleeDirection = Vector3.Lerp(transform.forward, fleeDirection, 0.7f);

        // Add a small random offset for natural movement
        Vector3 randomOffset = Random.insideUnitSphere * 3f;
        randomOffset.y = 0; // Keep it on the horizontal plane

        return transform.position + fleeDirection * 15f + randomOffset;
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

        if (homeTransform != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(homeTransform.position, maxDistanceFromHome);
        }
    }
}
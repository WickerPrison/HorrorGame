using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    IDLE, WANDERING, CHASING, ATTACKING, TESTING
}

public abstract class EnemyAi : MonoBehaviour
{
    public float visionRange;
    SpriteRenderer sprite;
    PlayerManager playerManager;
    LayerMask layerMask;
    List<PlayerUnit> unitsInSight = new List<PlayerUnit>();
    Seeker seeker;
    AIPath aiPath;
    [System.NonSerialized] public EnemyState state = EnemyState.IDLE;
    float idleMaxTime = 5f;
    float idleTimer;
    [SerializeField] float chaseSpeed;
    [SerializeField] float chaseSlowdownDistance;
    [SerializeField] float chaseEndReachedDistance;
    [SerializeField] float wanderSpeed;
    [SerializeField] float wanderSlowdownDistance;
    [SerializeField] float wanderEndReachedDistance;
    [SerializeField] float triggerAttackRange;
    float timeInCurrentRoom;
    [SerializeField] protected int damage;
    float maxDestinationTime = 7f;
    float destinationTime;

    protected virtual void Start()
    {
        playerManager = PlayerEvents.i.GetComponent<PlayerManager>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.enabled = false;
        layerMask = LayerMask.GetMask("Default", "Obstacle", "Player");
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        idleTimer = idleMaxTime;
        aiPath.maxSpeed = wanderSpeed;
        aiPath.slowdownDistance = wanderSlowdownDistance;
        aiPath.endReachedDistance = wanderEndReachedDistance;
    }

    private void Update()
    {
        sprite.enabled = VisionManager.i.FindIsVisible(transform.position);
        ManageCooldowns();

        if (state == EnemyState.TESTING) return;

        timeInCurrentRoom += Time.deltaTime;

        switch (state)
        {
            case EnemyState.CHASING:
                Aggro();
                if (unitsInSight.Count == 0)
                {
                    EndChasing();
                }
                else
                {
                    ChasePlayerUnits();
                }
                break;
            case EnemyState.IDLE:
                Aggro();
                idleTimer -= Time.deltaTime;
                if (idleTimer <= 0)
                {
                    GetWanderDestination();
                }
                break;
            case EnemyState.WANDERING:
                Aggro();
                destinationTime += Time.deltaTime;
                if (destinationTime > maxDestinationTime)
                {
                    GetWanderDestination();
                }
                if (Vector3.Distance(aiPath.destination, transform.position) <= 0.25f)
                {
                    state = EnemyState.IDLE;
                    idleTimer = idleMaxTime;
                }
                break;
            case EnemyState.ATTACKING:
                break;
        }
    }

    public virtual void Attack() { }

    protected virtual void ManageCooldowns() { }

    void Aggro()
    {
        unitsInSight.Clear();
        foreach (PlayerUnit unit in playerManager.allUnits)
        {
            if (unit == null) continue;
            float distance = Vector2.Distance(transform.position, unit.transform.position);
            if (distance <= visionRange)
            {
                Vector3 direction = unit.transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, unit.visionRange, layerMask);
                if (hit.transform != null && hit.transform.GetComponent<PlayerUnit>())
                {
                    unitsInSight.Add(unit);
                    state = EnemyState.CHASING;
                }
            }
        }
    }

    void GetWanderDestination()
    {
        Room room = Utils.GetRoom(transform.position);
        List<Room> accessibleRooms = room.GetAccessibleRooms();
        Room destinationRoom;
        if (timeInCurrentRoom > 15 && accessibleRooms.Count > 1)
        {
            destinationRoom = accessibleRooms[Random.Range(1, accessibleRooms.Count)];
        }
        else
        {
            destinationRoom = accessibleRooms[Random.Range(0, accessibleRooms.Count)];
        }
        Vector3 destination = destinationRoom.GetRandomPointInRoom();
        if (room != destinationRoom)
        {
            timeInCurrentRoom = 0;
        }
        GoTo(destination);
        state = EnemyState.WANDERING;
    }

    public void GoTo(Vector3 destination)
    {
        seeker.StartPath(transform.position, destination);
        aiPath.destination = destination;
        destinationTime = 0;
    }

    void ChasePlayerUnits()
    {
        aiPath.maxSpeed = chaseSpeed;
        aiPath.slowdownDistance = chaseSlowdownDistance;
        aiPath.endReachedDistance = chaseEndReachedDistance;
        PlayerUnit closestPlayer = GetClosestPlayer();

        if (closestPlayer != null)
        {
            aiPath.destination = closestPlayer.transform.position;
            if (Vector2.Distance(transform.position, closestPlayer.transform.position) < triggerAttackRange)
            {
                TriggerAttack();
            }
        }
        else
        {
            aiPath.destination = transform.position;
        }
    }

    protected PlayerUnit GetClosestPlayer()
    {
        float distance = 1000;
        PlayerUnit closestPlayer = null;
        foreach (PlayerUnit unit in unitsInSight)
        {
            float currentDistance = Vector2.Distance(unit.transform.position, transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closestPlayer = unit;
            }
        }
        return closestPlayer;
    }

    public virtual void TriggerAttack()
    {

    }

    void EndChasing()
    {
        state = EnemyState.IDLE;
        timeInCurrentRoom = 0;
        idleTimer = idleMaxTime;
        aiPath.maxSpeed = wanderSpeed;
        aiPath.slowdownDistance = wanderSlowdownDistance;
        aiPath.endReachedDistance = wanderEndReachedDistance;
    }

    public void SetTestingState()
    {
        state = EnemyState.TESTING;
    }

    public Vector3 TestingGetDestination()
    {
        return aiPath.destination;
    }
}

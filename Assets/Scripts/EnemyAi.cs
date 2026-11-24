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
    protected LayerMask layerMask;
    protected List<IFightDemons> demonFightersInSight = new List<IFightDemons>();
    Seeker seeker;
    protected AIPath aiPath;
    [System.NonSerialized] public EnemyState state = EnemyState.IDLE;
    float idleMaxTime = 5f;
    float idleTimer;
    [SerializeField] float chaseSpeed;
    [SerializeField] float chaseSlowdownDistance;
    [SerializeField] float chaseEndReachedDistance;
    [SerializeField] float wanderSpeed;
    [SerializeField] float wanderSlowdownDistance;
    [SerializeField] float wanderEndReachedDistance;
    [SerializeField] protected float triggerAttackRange;
    float timeInCurrentRoom;
    float maxDestinationTime = 7f;
    float destinationTime;
    protected Enemy enemy;
    protected IFightDemons closestDemonFighter;

    protected virtual void Start()
    {
        playerManager = PlayerEvents.i.GetComponent<PlayerManager>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.enabled = false;
        layerMask = LayerMask.GetMask("Default", "Obstacle", "Player", "Enemy");
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        idleTimer = idleMaxTime;
        aiPath.maxSpeed = wanderSpeed;
        aiPath.slowdownDistance = wanderSlowdownDistance;
        aiPath.endReachedDistance = wanderEndReachedDistance;
        enemy = GetComponent<Enemy>();
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
                if (LostTargets())
                {
                    EndChasing();
                }
                else
                {
                    ChaseDemonFighters();
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

    protected virtual void Aggro()
    {
        demonFightersInSight.Clear();
        foreach (IFightDemons demonFighter in MissionManager.i.demonFighters)
        {
            if (demonFighter == null) continue;
            float distance = Vector2.Distance(transform.position, demonFighter.transform.position);
            if (distance <= visionRange)
            {
                Vector3 direction = demonFighter.transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, visionRange, layerMask);
                if (hit.transform != null && hit.transform.GetComponent<IFightDemons>() == demonFighter)
                {
                    demonFightersInSight.Add(demonFighter);
                    state = EnemyState.CHASING;
                }
            }
        }
    }

    protected virtual bool LostTargets()
    {
        return demonFightersInSight.Count == 0;
    }

    protected virtual void GetWanderDestination()
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

    protected virtual void ChaseDemonFighters()
    {
        aiPath.maxSpeed = chaseSpeed;
        aiPath.slowdownDistance = chaseSlowdownDistance;
        aiPath.endReachedDistance = chaseEndReachedDistance;
        IFightDemons closestDemonFighter = GetClosestDemonFighter();

        if (closestDemonFighter != null)
        {
            aiPath.destination = closestDemonFighter.transform.position;
            if (Vector2.Distance(transform.position, closestDemonFighter.transform.position) < triggerAttackRange)
            {
                TriggerAttack();
            }
        }
        else
        {
            aiPath.destination = transform.position;
        }
    }

    protected IFightDemons GetClosestDemonFighter()
    {
        float distance = 1000;
        closestDemonFighter = null;
        foreach (IFightDemons demonFighter in demonFightersInSight)
        {
            float currentDistance = Vector2.Distance(demonFighter.transform.position, transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closestDemonFighter = demonFighter;
            }
        }
        return closestDemonFighter;
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

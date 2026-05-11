using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    IDLE, WANDERING, CHASING, TESTING
}

public class Enemy : MonoBehaviour, ITakeDamage, IGetTeleported
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
    float timeInCurrentRoom;
    [SerializeField] int damage;
    public int maxHealth;
    [System.NonSerialized] public int health;
    [SerializeField] GameObject deadEnemyPrefab;

    void Start()
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
        health = maxHealth;
    }

    private void Update()
    {
        sprite.enabled = VisionManager.i.FindIsVisible(transform.position);

        if (state == EnemyState.TESTING) return;

        Aggro();

        timeInCurrentRoom += Time.deltaTime;

        switch (state)
        {
            case EnemyState.CHASING:
                if(unitsInSight.Count == 0)
                {
                    EndChasing();
                }
                else
                {
                    ChasePlayerUnits();
                }
                break;
            case EnemyState.IDLE:
                idleTimer -= Time.deltaTime;
                if(idleTimer <= 0)
                {
                    GetWanderDestination();
                }
                break;
            case EnemyState.WANDERING:
                if(Vector3.Distance(aiPath.destination, transform.position) <= 0.25f)
                {
                    state = EnemyState.IDLE;
                    idleTimer = idleMaxTime;
                }
                break;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Instantiate(deadEnemyPrefab).transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    void Aggro()
    {
        unitsInSight.Clear();
        foreach (PlayerUnit unit in playerManager.allUnits)
        {
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
        if(room != destinationRoom)
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
    }

    void ChasePlayerUnits()
    {
        aiPath.maxSpeed = chaseSpeed;
        aiPath.slowdownDistance = chaseSlowdownDistance;
        aiPath.endReachedDistance = chaseEndReachedDistance;
        float distance = 1000;
        PlayerUnit closestPlayer = null;
        foreach(PlayerUnit unit in unitsInSight)
        {
            float currentDistance = Vector2.Distance(unit.transform.position, transform.position);
            if(currentDistance < distance)
            {
                distance = currentDistance;
                closestPlayer = unit;
            }
        }

        if(closestPlayer != null)
        {
            aiPath.destination = closestPlayer.transform.position;
        }
        else
        {
            aiPath.destination = transform.position;
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerUnit>(out PlayerUnit playerUnit))
        {
            playerUnit.TakeDamage(damage);
        }
    }

    public void GotTeleported()
    {
        //this is just to make the interface happy.
    }

    public void LeaveMission()
    {
        Destroy(gameObject);
        //TODO: let players kidnap demons
    }

    public void SetTestingState()
    {
        state = EnemyState.TESTING;
    }
}

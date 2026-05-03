using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    IDLE, WANDERING, CHASING
}

public class Enemy : MonoBehaviour
{
    [SerializeField] float visionRange;
    SpriteRenderer sprite;
    PlayerManager playerManager;
    LayerMask layerMask;
    List<PlayerUnit> unitsInSight = new List<PlayerUnit>();
    Seeker seeker;
    AIPath aiPath;
    EnemyState state = EnemyState.IDLE;
    float idleMaxTime = 5f;
    float idleTimer;
    [SerializeField] float chaseSpeed;
    [SerializeField] float chaseSlowdownDistance;
    [SerializeField] float chaseEndReachedDistance;
    [SerializeField] float wanderSpeed;
    [SerializeField] float wanderSlowdownDistance;
    [SerializeField] float wanderEndReachedDistance;

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
    }

    private void Update()
    {
        Vision();


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

    void Vision()
    {
        bool showSprite = false;
        unitsInSight.Clear();
        foreach (PlayerUnit unit in playerManager.allUnits)
        {
            float distance = Vector2.Distance(transform.position, unit.transform.position);
            if (distance <= visionRange || (distance <= unit.visionRange && !showSprite))
            {
                Vector3 direction = unit.transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, unit.visionRange, layerMask);
                if (hit.transform != null && hit.transform.GetComponent<PlayerUnit>())
                {
                    if(distance <= unit.visionRange)
                    {
                        showSprite = true;
                    }
                    if(distance <= visionRange)
                    {
                        unitsInSight.Add(unit);
                        state = EnemyState.CHASING;
                    }
                }
            }
        }
        sprite.enabled = showSprite;
    }

    void GetWanderDestination()
    {
        Room room = Utils.GetRoom(transform.position);
        List<Room> accessibleRooms = room.GetAccessibleRooms();
        Room destinationRoom = accessibleRooms[Random.Range(0, accessibleRooms.Count)];
        Vector3 destination = destinationRoom.GetRandomPointInRoom();
        seeker.StartPath(transform.position, destination);
        aiPath.destination = destination;
        state = EnemyState.WANDERING;
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
        idleTimer = idleMaxTime;
        aiPath.maxSpeed = wanderSpeed;
        aiPath.slowdownDistance = wanderSlowdownDistance;
        aiPath.endReachedDistance = wanderEndReachedDistance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.TryGetComponent<PlayerUnit>(out PlayerUnit playerUnit))
        {
            playerUnit.Death();
        }
    }
}

using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float visionRange;
    SpriteRenderer sprite;
    PlayerManager playerManager;
    LayerMask layerMask;
    List<PlayerUnit> unitsInSight = new List<PlayerUnit>();
    AIPath aiPath;

    void Start()
    {
        playerManager = PlayerEvents.i.GetComponent<PlayerManager>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        Utils.GetRooms(transform.position, 0.1f)[0].enemies.Add(this);
        layerMask = LayerMask.GetMask("Default", "Obstacle", "Player");
        aiPath = GetComponent<AIPath>();
    }

    private void Update()
    {
        Vision();

        ChasePlayerUnits();
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
                    }
                }
            }
        }
        sprite.enabled = showSprite;
    }

    void ChasePlayerUnits()
    {
        if (unitsInSight.Count == 0) return;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.TryGetComponent<PlayerUnit>(out PlayerUnit playerUnit))
        {
            playerUnit.Death();
        }
    }
}

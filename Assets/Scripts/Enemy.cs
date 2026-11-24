using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITakeDamage, IGetTeleported
{
    public int maxHealth;
    [System.NonSerialized] public int health;
    [SerializeField] GameObject deadEnemyPrefab;

    void Start()
    {
        health = maxHealth;
        MissionEvents.i.EnemyExists(this);
    }

    public void TakeHolyAuraDamage(int holyAuraLevel)
    {
        TakeDamage(holyAuraLevel + 10);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        Instantiate(deadEnemyPrefab).transform.position = transform.position;
        MissionEvents.i.EnemyDeath(this);
        Destroy(gameObject);
    }

    public void GotTeleported()
    {
        //this is just to make the interface happy.
    }

    public void LeaveMission()
    {
        MissionEvents.i.EnemyDeath(this);
        Destroy(gameObject);
        //TODO: let players kidnap demons
    }
}

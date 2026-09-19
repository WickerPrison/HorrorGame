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
            Instantiate(deadEnemyPrefab).transform.position = transform.position;
            Destroy(gameObject);
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
}

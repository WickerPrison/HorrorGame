using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager i;

    public List<IFightDemons> demonFighters = new List<IFightDemons>();
    public List<Enemy> enemies = new List<Enemy>();

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;
    }

    private void OnEnable()
    {
        MissionEvents.i.onDemonFighterExists += Mission_onDemonFighterExists;
        MissionEvents.i.onDemonFighterDeath += Mission_onDemonFighterDeath;
        MissionEvents.i.onEnemyExists += Mission_onEnemyExists;
        MissionEvents.i.onEnemyDeath += Mission_onEnemyDeath;
    }

    private void OnDisable()
    {
        MissionEvents.i.onDemonFighterExists -= Mission_onDemonFighterExists;
        MissionEvents.i.onDemonFighterDeath -= Mission_onDemonFighterDeath;
        MissionEvents.i.onEnemyExists -= Mission_onEnemyExists;
        MissionEvents.i.onEnemyDeath -= Mission_onEnemyDeath;
    }

    private void Mission_onDemonFighterExists(IFightDemons demonFighter)
    {
        demonFighters.Add(demonFighter);
    }

    private void Mission_onDemonFighterDeath(IFightDemons demonFighter)
    {
        demonFighters.Remove(demonFighter);
    }

    private void Mission_onEnemyExists(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    private void Mission_onEnemyDeath(Enemy enemy)
    {
        enemies.Remove(enemy);
    }
}

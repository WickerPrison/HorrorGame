using System.Collections.Generic;
using UnityEngine;

public class DemonAi : EnemyAi, IFightDemons
{
    [SerializeField] SlashAnimation slashAnimation;
    [SerializeField] int clawDamage;
    float attackCd = 1f;
    float attackTimer = 0;
    ITakeDamage attackTarget;
    List<Enemy> enemiesInSight = new List<Enemy>();

    private void OnEnable()
    {
        MissionEvents.i.onEnemyDeath += Mission_onEnemyDeath;
    }

    private void OnDisable()
    {
        MissionEvents.i.onEnemyDeath -= Mission_onEnemyDeath;
    }

    protected override void Start()
    {
        base.Start();
        RegisterWithMissionManager();
        slashAnimation.stopAttackingMethod = EndAttacking;
    }

    protected override void ManageCooldowns()
    {
        base.ManageCooldowns();
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
    }

    protected override void Aggro()
    {
        base.Aggro();
        enemiesInSight.Clear();
        foreach(Enemy enemy in MissionManager.i.enemies)
        {
            if (enemy == null) continue;
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance <= visionRange)
            {
                Vector3 direction = enemy.transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, visionRange, layerMask);
                if (hit.transform != null && hit.transform.GetComponent<Enemy>() == enemy)
                {
                    enemiesInSight.Add(enemy);
                    state = EnemyState.CHASING;
                }
            }
        }
    }

    protected override void ChaseDemonFighters()
    {
        base.ChaseDemonFighters();
        Enemy closestEnemy = GetClosestEnemy();
        if (closestEnemy == null) return;
        float enemyDistance = Vector3.Distance(transform.position, closestEnemy.transform.position);

        bool enemyIsCloser = closestDemonFighter == null ? true : enemyDistance < Vector2.Distance(transform.position, closestDemonFighter.transform.position);

        if (!enemyIsCloser) return;

        aiPath.destination = closestEnemy.transform.position;

        if(enemyDistance < triggerAttackRange)
        {
            TriggerAttack();
        }

    }

    protected override bool LostTargets()
    {
        return demonFightersInSight.Count == 0 && enemiesInSight.Count == 0;
    }

    Enemy GetClosestEnemy()
    {
        float distance = 1000;
        Enemy closestEnemy = null;
        foreach (Enemy enemy in enemiesInSight)
        {
            float currentDistance = Vector2.Distance(enemy.transform.position, transform.position);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }

    ITakeDamage GetClosestTarget()
    {
        Enemy closestEnemy = GetClosestEnemy();
        IFightDemons closestDemonFighter = GetClosestDemonFighter();
        if (closestEnemy == null) return closestDemonFighter;
        if (closestDemonFighter == null) return closestEnemy;
        float enemyDist = Vector2.Distance(transform.position, closestEnemy.transform.position);
        float demonFighterDist = Vector2.Distance(transform.position, closestDemonFighter.transform.position);
        if (enemyDist < demonFighterDist)
        {
            return closestEnemy;
        }
        else
        {
            return closestDemonFighter;
        }
    }

    public override void TriggerAttack()
    {
        base.Attack();
        if (attackTimer > 0) return;
        attackTimer = attackCd;
        GoTo(transform.position);
        state = EnemyState.ATTACKING;
        attackTarget = GetClosestTarget();
        slashAnimation.Animation(transform.position, attackTarget.transform, HitTarget);
    }

    void HitTarget(bool hit)
    {
        if (!hit) return;
        Debug.Log(attackTarget);
        attackTarget.TakeDamage(clawDamage);
    }

    void EndAttacking()
    {
        attackTarget = null;
        state = EnemyState.CHASING;
    }

    public void RegisterWithMissionManager()
    {
        MissionEvents.i.DemonFighterExists(this);
    }

    public void RemoveFromMissionManager()
    {
        MissionEvents.i.DemonFighterDeath(this);
    }

    public void TakeDamage(int damage)
    {
        enemy.TakeDamage(damage);
    }

    private void Mission_onEnemyDeath(Enemy enemy)
    {
        if (enemy != this.enemy) return;
        RemoveFromMissionManager();
    }
}

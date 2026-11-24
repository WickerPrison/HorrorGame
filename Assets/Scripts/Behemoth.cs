using System.Collections.Generic;
using UnityEngine;

public class Behemoth : EnemyAi
{
    [SerializeField] SlashAnimation slashAnimation;
    [SerializeField] int clawDamage;
    float attackCd = 2f;
    float attackTimer = 0;
    IFightDemons attackTarget;

    protected override void Start()
    {
        base.Start();
        slashAnimation.stopAttackingMethod = EndAttacking;
    }

    protected override void ManageCooldowns()
    {
        base.ManageCooldowns();
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
    }

    protected override void GetWanderDestination()
    {
        Room room = Utils.GetRoom(transform.position);
        Vector3 destination = room.GetRandomPointInRoom();
        GoTo(destination);
        state = EnemyState.WANDERING;
    }

    public override void TriggerAttack()
    {
        base.Attack();
        if (attackTimer > 0) return;
        attackTimer = attackCd;
        GoTo(transform.position);
        state = EnemyState.ATTACKING;
        attackTarget = GetClosestDemonFighter();
        slashAnimation.Animation(transform.position, attackTarget.transform, HitTarget);
    }

    void HitTarget(bool hit)
    {
        if (!hit) return;
        attackTarget.TakeDamage(clawDamage);
    }

    void EndAttacking()
    {
        attackTarget = null;
        state = EnemyState.CHASING;
    }
}

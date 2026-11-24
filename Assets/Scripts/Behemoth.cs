using System.Collections.Generic;
using UnityEngine;

public class Behemoth : EnemyAi
{
    [SerializeField] SlashAnimation slashAnimation;
    float attackCd = 0.9f;
    float attackTimer = 0;

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
        GoTo(transform.position);
        state = EnemyState.ATTACKING;
        PlayerUnit target = GetClosestPlayer();
        slashAnimation.Animation(transform.position, target.transform.position - transform.position);
    }
}

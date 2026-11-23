using System.Collections.Generic;
using UnityEngine;

public class Behemoth : EnemyAi
{
    [SerializeField] SlashAnimation slashAnimation;

    protected override void GetWanderDestination()
    {
        Room room = Utils.GetRoom(transform.position);
        Vector3 destination = room.GetRandomPointInRoom();
        GoTo(destination);
        state = EnemyState.WANDERING;
    }

    public override void Attack()
    {
        base.Attack();
        //slashAnimation.Animation()
    }
}

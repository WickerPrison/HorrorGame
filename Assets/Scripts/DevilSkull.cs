using UnityEngine;

public class DevilSkullAi : EnemyAi
{
    [SerializeField] int damage;

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        Room room = Utils.GetRoom(transform.position);
        room.DamageRoom(60);
    }
}

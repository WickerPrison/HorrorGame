using System.Collections;
using UnityEngine;

public class ImpAi : EnemyAi
{
    [SerializeField] GameObject fireballPrefab;
    WaitForSeconds attackDelay = new WaitForSeconds(0.2f);
    float attackCd = 0.9f;
    float attackTimer = 0;
    [SerializeField] int fireballDamage;

    protected override void ManageCooldowns()
    {
        base.ManageCooldowns();
        if (attackTimer > 0) attackTimer -= Time.deltaTime;
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();
        if (attackTimer > 0) return;
        attackTimer = attackCd;
        GoTo(transform.position);
        state = EnemyState.ATTACKING;
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        yield return attackDelay;
        PlayerUnit target = GetClosestPlayer();
        Vector3 direction = Vector3.Normalize(target.transform.position - transform.position);
        Fireball.Instantiate(fireballPrefab, transform.position, direction, fireballDamage);
        yield return attackDelay;
        state = EnemyState.CHASING;
    }
}

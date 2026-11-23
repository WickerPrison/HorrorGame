using UnityEngine;

public class BasicEnemyAi : EnemyAi
{
    [SerializeField] int damage;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerUnit>(out PlayerUnit playerUnit))
        {
            playerUnit.TakeDamage(damage);
        }
    }
}

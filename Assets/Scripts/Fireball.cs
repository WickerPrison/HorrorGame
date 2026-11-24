using UnityEngine;

public class Fireball : Projectile
{
    int damage;

    public static Fireball Instantiate(GameObject prefab, Vector3 position, Vector3 direction, int damage)
    {
        Fireball fireball = (Fireball)Projectile.Instantiate(prefab, position, direction);
        fireball.damage = damage;
        return fireball;
    }

    protected override void HitCollider(Collider2D collider)
    {
        if (collider.gameObject.TryGetComponent<PlayerUnit>(out PlayerUnit playerUnit))
        {
            playerUnit.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}

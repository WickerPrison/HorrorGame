using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] LayerMask collisionMask;
    Vector3 direction;
    bool instantiatedCorrectly = false;

    public static Projectile Instantiate(GameObject prefab, Vector3 position, Vector3 direction)
    {
        Projectile projectile = Instantiate(prefab).GetComponent<Projectile>();
        projectile.transform.position = position;
        projectile.direction = direction;
        projectile.instantiatedCorrectly = true;
        return projectile;
    }

    private void Start()
    {
        if (!instantiatedCorrectly) Utils.IncorrectInitialization("Projectile");
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        Vector3 newPosition = transform.position + direction.normalized * speed * Time.fixedDeltaTime;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, newPosition, collisionMask);
        if(hit)
        {
            HitCollider(hit.collider);
        }
        else
        {
            transform.position = newPosition;
        }
    }

    protected virtual void HitCollider(Collider2D collider)
    {
        Destroy(gameObject);
    }
}

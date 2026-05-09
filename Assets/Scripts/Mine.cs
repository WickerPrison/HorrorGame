using UnityEngine;

public class Mine : MonoBehaviour, IUnhideWhenSeen
{
    [SerializeField] int damage;
    [SerializeField] float detonateRange;
    [SerializeField] ColorData colorData;
    SpriteRenderer sprite;
    Room myRoom;

    private void Start()
    {
        myRoom = Utils.GetRoom(transform.position);
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.color = colorData.player;
        sprite.enabled = false;
    }

    public void Unhide()
    {
        sprite.enabled = true;
    }

    private void Update()
    {
        foreach(Enemy enemy in myRoom.enemies)
        {
            if(Vector2.Distance(transform.position, enemy.transform.position) <= detonateRange)
            {
                myRoom.DamageRoom(damage);
                Destroy(gameObject);
                return;
            }
        }
    }
}

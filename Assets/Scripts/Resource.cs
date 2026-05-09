using UnityEngine;

public class Resource : MonoBehaviour, IUnhideWhenSeen, ITakeDamage
{
    Room room;
    int value;
    SpriteRenderer sprite;
    PlayerManager playerManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerManager = PlayerEvents.i.GetComponent<PlayerManager>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.enabled = false;
        room = Utils.GetRoom(transform.position);
        room.resources.Add(this);

        value = Random.Range(1, 5);
    }

    public void Unhide()
    {
        sprite.enabled = true;
    }

    public void GetCollected()
    {
        playerManager.GainResources(value);
        room.resources.Remove(this);
        Destroy(gameObject);
    }

    public void TakeDamage(int _)
    {
        room.resources.Remove(this);
        Destroy(gameObject);
    }
}

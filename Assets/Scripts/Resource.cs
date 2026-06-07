using System.Linq;
using UnityEngine;

public class Resource : MonoBehaviour, IUnhideWhenSeen, ITakeDamage, IInterceptRightClick
{
    Room room;
    int value;
    SpriteRenderer sprite;
    PlayerManager playerManager;
    [SerializeField] GameObject destroyedResourcePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerManager = PlayerEvents.i.GetComponent<PlayerManager>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.enabled = false;
        room = Utils.GetRoom(transform.position);
        room.resources.Add(this);
        room.AddDamageTaker(this);

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
        room.RemoveDamageTaker(this);
        Destroy(gameObject);
    }

    public void TakeDamage(int _)
    {
        room.resources.Remove(this);
        room.RemoveDamageTaker(this);
        Instantiate(destroyedResourcePrefab).transform.position = transform.position;
        Destroy(gameObject);
    }

    public bool RightClick()
    {
        if(playerManager.selectedUnits.Count != 1 || !playerManager.selectedUnits[0].data.abilities.Contains(Ability.COLLECT))
        {
            return true;
        }

        playerManager.selectedUnits[0].unitAbilities.Collect(this);
        return false;
    }
}

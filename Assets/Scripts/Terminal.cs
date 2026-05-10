using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terminal : MonoBehaviour, IUnhideWhenSeen, IPowerRooms, IInterceptRightClick
{
    SpriteRenderer[] sprites;
    Room room;
    public List<Room> roomsToPower = new List<Room>();
    [SerializeField] Transform interactPoint;
    PlayerManager playerManager;

    void Start()
    {
        playerManager = PlayerEvents.i.GetComponent<PlayerManager>();
        room = Utils.GetRoom(transform.position);
        room.terminal = this;
        sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.enabled = false;
        }
    }

    public void Unhide()
    {
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.enabled = true;
        }
    }

    public Vector3 GetInteractPoint()
    {
        return interactPoint.position;
    }

    public void StartPowering()
    {
        foreach(Room room in roomsToPower)
        {
            room.AddPower(this);
        }
    }

    public void EndPowering()
    {
        foreach (Room room in roomsToPower)
        {
            room.LosePower(this);
        }
    }

    public bool RightClick()
    {
        if(playerManager.selectedUnits.Count != 1 || !playerManager.selectedUnits[0].data.abilities.Contains(Ability.POWER))
        {
            return true;
        }

        playerManager.selectedUnits[0].unitAbilities.Power(this);
        return false;
    }
}

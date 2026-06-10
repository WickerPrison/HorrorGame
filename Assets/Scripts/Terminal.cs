using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction
{
    LEFT, RIGHT, UP, DOWN
}

public class Terminal : MonoBehaviour, IUnhideWhenSeen, IPowerRooms, IInterceptRightClick
{
    SpriteRenderer[] sprites;
    Room room;
    public List<Room> roomsToPower = new List<Room>();
    [SerializeField] Transform interactPoint;
    PlayerManager playerManager;
    [SerializeField] Direction interactPointDirection;
    float interactPointDistance = 0.6f;
    [SerializeField] GameObject roomFinders;

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

        GetPoweredRooms();

        Vector3 interactPointOffset = interactPointDirection switch
        {
            Direction.LEFT => Vector3.left * interactPointDistance,
            Direction.RIGHT => Vector3.right * interactPointDistance,
            Direction.UP => Vector3.up * interactPointDistance,
            Direction.DOWN => Vector3.down * interactPointDistance
        };
        interactPoint.position = transform.position + interactPointOffset;
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
        if(playerManager.selectedUnits.Count != 1 || !playerManager.selectedUnits[0].data.CanUseAbilityType(AbilityType.POWER))
        {
            return true;
        }
        playerManager.selectedUnits[0].unitAbilities.Power(this);
        return false;
    }

    void GetPoweredRooms()
    {
        foreach(Transform roomFinder in roomFinders.GetComponentsInChildren<Transform>())
        {
            Room room = Utils.GetRoom(roomFinder.position);
            if (!roomsToPower.Contains(room))
            {
                roomsToPower.Add(room);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour, IUnhideWhenSeen, IPowerRooms
{
    SpriteRenderer[] sprites;
    Room room;
    public List<Room> roomsToPower = new List<Room>();
    [SerializeField] Transform interactPoint;

    void Start()
    {
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
        Debug.Log("end power");
        foreach (Room room in roomsToPower)
        {
            room.LosePower(this);
        }
    }
}

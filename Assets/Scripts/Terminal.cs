using System.Collections.Generic;
using UnityEngine;

public class Terminal : MonoBehaviour, IUnhideWhenSeen, IPowerRooms
{
    SpriteRenderer[] sprites;
    Room room;
    public List<Room> roomsToPower = new List<Room>();

    void Start()
    {
        room = Utils.GetRoom(transform.position);
        room.terminals.Add(this);
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

    public void StartHacking()
    {
        foreach(Room room in roomsToPower)
        {
            room.AddPower(this);
        }
    }

    public void EndHacking()
    {
        Debug.Log("end hacking");
        foreach (Room room in roomsToPower)
        {
            room.LosePower(this);
        }
    }
}

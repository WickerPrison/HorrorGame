using UnityEngine;

[RequireComponent(typeof(Room))]
public class InnatelyPoweredRoom : MonoBehaviour, IPowerRooms
{
    Room room;

    void Start()
    {
        room = GetComponent<Room>();
        room.AddPower(this);
    }
}

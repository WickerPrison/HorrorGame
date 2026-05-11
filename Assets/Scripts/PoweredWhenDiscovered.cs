using UnityEngine;

[RequireComponent(typeof(Room))]
public class PoweredWhenDiscovered : MonoBehaviour, IPowerRooms
{
    Room room;
    bool powering = false;

    void Awake()
    {
        room = GetComponent<Room>();
    }

    private void OnEnable()
    {
        room.onChangeState += Room_onChangeState;
    }

    private void OnDisable()
    {
        room.onChangeState -= Room_onChangeState;
    }

    private void Room_onChangeState(RoomState state)
    {
        if(!powering && state != RoomState.HIDDEN)
        {
            room.AddPower(this);
        }
    }
}

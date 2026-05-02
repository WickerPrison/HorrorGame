using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum DoorState
{
    OPEN, CLOSED, OPENING, CLOSING
}

public class Door : MonoBehaviour
{
    DoorState state = DoorState.CLOSED;
    List<Room> rooms;

    public event System.Action<float> onDoorProgress;
    float doorProgress = 0;
    float doorSpeed = 5;

    private void Start()
    {
        rooms = Utils.GetRooms(transform.position);
        foreach(Room room in rooms)
        {
            room.onChangeState += Room_onChangeState;
            foreach(Room adjacentRoom in rooms)
            {
                if(adjacentRoom != room)
                {
                    room.adjacentRooms.Add(adjacentRoom);
                }
            }
        }
        onDoorProgress?.Invoke(doorProgress);
    }

    private void Update()
    {
        switch (state)
        {
            case DoorState.OPENING:
                doorProgress += Time.deltaTime * doorSpeed;
                if(doorProgress >= 1)
                {
                    doorProgress = 1;
                    state = DoorState.OPEN;
                    onDoorProgress?.Invoke(doorProgress);
                }
                onDoorProgress?.Invoke(doorProgress);
                break;
            case DoorState.CLOSING:
                doorProgress -= Time.deltaTime * doorSpeed;
                if (doorProgress <= 0)
                {
                    doorProgress = 0;
                    state = DoorState.CLOSED;
                }
                onDoorProgress?.Invoke(doorProgress);
                break;
        }
    }

    private void Room_onChangeState(RoomState newState)
    {
        //if (newState != RoomState.HIDDEN) sprite.enabled = true;
    }

    private void OnEnable()
    {
        InputManager.i.onRightClick += RightClick;
    }

    private void OnDisable()
    {
        InputManager.i.onRightClick -= RightClick;
        foreach (Room room in rooms)
        {
            room.onChangeState -= Room_onChangeState;
        }
    }

    void RightClick(Vector3 worldPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(worldPos, Layers.clickableMask);
        if (hit != null && hit.gameObject == gameObject)
        {
            if(state == DoorState.OPEN || state == DoorState.OPENING)
            {
                state = DoorState.CLOSING;
            }
            else
            {
                state = DoorState.OPENING;
            }
        }
    }
}

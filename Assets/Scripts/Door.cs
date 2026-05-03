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
    public Dictionary<Room, Room> roomDict = new Dictionary<Room, Room>();

    public event System.Action<float> onDoorProgress;
    float doorProgress = 0;
    float doorSpeed = 5;

    private void Start()
    {
        rooms = Utils.GetRooms(transform.position);
        foreach(Room room in rooms)
        {
            room.onChangeState += Room_onChangeState;
            room.doors.Add(this);
        }
        roomDict.Add(rooms[0], rooms[1]);
        roomDict.Add(rooms[1], rooms[0]);
        onDoorProgress?.Invoke(doorProgress);

        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.enabled = false;
        }
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

    public Room GetAccessibleRoom(Room caller)
    {
        if(state == DoorState.OPEN)
        {
            return roomDict[caller];
        }
        return null;
    }

    private void Room_onChangeState(RoomState newState)
    {
        if (newState != RoomState.HIDDEN)
        {
            SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer sprite in spriteRenderers)
            {
                sprite.enabled = true;
            }
        }
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

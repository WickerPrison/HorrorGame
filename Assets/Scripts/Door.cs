using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum DoorState
{
    OPEN, CLOSED, OPENING, CLOSING
}

public class Door : MonoBehaviour, IInterceptRightClick
{
    [SerializeField] ColorData colorData;
    DoorState state = DoorState.CLOSED;
    List<Room> rooms;
    public Dictionary<Room, Room> roomDict = new Dictionary<Room, Room>();

    public event System.Action<float> onDoorProgress;
    [SerializeField] bool startOpen;
    float doorProgress = 0;
    float doorSpeed = 5;
    bool powered;

    [SerializeField] SpriteRenderer[] spriteRenderers;

    private void Start()
    {
        if (startOpen) doorProgress = 1;
        onDoorProgress?.Invoke(doorProgress);

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        UpdatePowerState();
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

    public void UpdatePowerState()
    {
        powered = rooms[0].HasPower() || rooms[1].HasPower();
        Color color = powered ? colorData.powered : colorData.unpowered;

        foreach (SpriteRenderer sprite in spriteRenderers)
        {
            sprite.color = color;
        }
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
        rooms = Utils.GetRooms(transform.position);
        foreach (Room room in rooms)
        {
            room.onChangeState += Room_onChangeState;
            room.doors.Add(this);
        }
        roomDict.Add(rooms[0], rooms[1]);
        roomDict.Add(rooms[1], rooms[0]);
    }

    private void OnDisable()
    {
        foreach (Room room in rooms)
        {
            room.onChangeState -= Room_onChangeState;
        }
    }

    public bool RightClick()
    {
        if (!powered) return true;

        if(state == DoorState.OPEN || state == DoorState.OPENING)
        {
            state = DoorState.CLOSING;
        }
        else
        {
            state = DoorState.OPENING;
        }
        return false;
    }
}

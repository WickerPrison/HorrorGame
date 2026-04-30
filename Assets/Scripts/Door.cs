using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum DoorState
{
    OPEN, CLOSED
}

public class Door : MonoBehaviour
{
    DoorState state = DoorState.CLOSED;
    [SerializeField] Collider2D navCollider;
    SpriteRenderer sprite;
    List<Room> rooms;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
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
    }

    private void Room_onChangeState(RoomState newState)
    {
        if (newState != RoomState.HIDDEN) sprite.enabled = true;
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
            if(state == DoorState.OPEN)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
    }

    void OpenDoor()
    {
        state = DoorState.OPEN;
        navCollider.enabled = false;
        sprite.color = Color.white;
    }

    void CloseDoor()
    {
        state = DoorState.CLOSED;
        navCollider.enabled = true;
        sprite.color = Color.blue;
    }
}

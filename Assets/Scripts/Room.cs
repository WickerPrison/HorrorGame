using System.Collections.Generic;
using UnityEngine;

public enum RoomState
{
    HIDDEN, DISCOVERED, EXPLORED
}

public enum ScanningState
{
    UNSCANNED, DANGER, SAFE
}

public class Room : MonoBehaviour
{
    public List<Door> doors;
    public List<Enemy> enemies;
    [SerializeField] Transform wallsParent;
    List<Wall> walls = new List<Wall>();
    RoomState state = RoomState.HIDDEN;
    public event System.Action<RoomState> onChangeState;
    List<PlayerUnit> scanningUnits = new List<PlayerUnit>();
    [System.NonSerialized] public List<Resource> resources = new List<Resource>();
    [SerializeField] bool alwaysPowered;
    public bool powered;
    BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        foreach(Transform child in wallsParent)
        {
            walls.Add(child.GetComponent<Wall>());
        }

        foreach(Wall wall in walls)
        {
            wall.SpriteVisible(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state != RoomState.EXPLORED && collision.CompareTag("Player"))
        {
            SetState(RoomState.EXPLORED);
            return;
        }

        if(collision.TryGetComponent<Enemy>(out Enemy enteringEnemy))
        {
            enemies.Add(enteringEnemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Enemy>(out Enemy leavingEnemy))
        {
            enemies.Remove(leavingEnemy);
        }
    }

    void SetState(RoomState newState)
    {
        switch (newState)
        {
            case RoomState.EXPLORED:
                state = RoomState.EXPLORED;
                onChangeState?.Invoke(RoomState.EXPLORED);
                foreach(Wall wall in walls)
                {
                    wall.SpriteVisible(true);
                }
                break;
            case RoomState.DISCOVERED:
                state = RoomState.EXPLORED;
                onChangeState?.Invoke(RoomState.EXPLORED);
                foreach (Wall wall in walls)
                {
                    wall.SpriteVisible(true);
                }
                break;
        }
    }

    public List<Room> GetAccessibleRooms()
    {
        List<Room> accessibleRooms = new List<Room> { this };
        foreach(Door door in doors)
        {
            Room otherRoom = door.GetAccessibleRoom(this);
            if (otherRoom != null) accessibleRooms.Add(otherRoom);
        }
        return accessibleRooms;
    }

    public Vector3 GetRandomPointInRoom()
    {
        float halfWidth = boxCollider.size.x / 2;
        float halfHeight = boxCollider.size.y / 2;
        float xOffset = Random.Range(-halfWidth, halfWidth);
        float yOffset = Random.Range(-halfHeight, halfHeight);
        return transform.position + new Vector3(xOffset, yOffset, 0);
    }

    public void ScanAdjacentRooms(PlayerUnit scanningUnit)
    {
        foreach(Door door in doors)
        {
            door.roomDict[this].GetScanned(scanningUnit);
        }
    }

    public void GetScanned(PlayerUnit scanningUnit)
    {
        if (state == RoomState.HIDDEN) SetState(RoomState.DISCOVERED);
        if (!scanningUnits.Contains(scanningUnit))
        {
            scanningUnits.Add(scanningUnit);
        }
        if(enemies.Count == 0)
        {
            SetWallScanState(ScanningState.SAFE);
        }
        else
        {
            SetWallScanState(ScanningState.DANGER);
        }
    }

    void StopGettingScanned()
    {
        SetWallScanState(ScanningState.UNSCANNED);
    }

    void SetWallScanState(ScanningState setState)
    {
        foreach(Wall wall in walls)
        {
            wall.SetScanState(setState);
        }
    }

    private void Global_onUnitStopScanning(PlayerUnit scanningUnit)
    {
        scanningUnits.Remove(scanningUnit);
        if(scanningUnits.Count == 0)
        {
            StopGettingScanned();
        }
    }

    private void OnEnable()
    {
        GlobalEvents.i.onUnitStopScanning += Global_onUnitStopScanning;
    }

    private void OnDisable()
    {
        GlobalEvents.i.onUnitStopScanning -= Global_onUnitStopScanning;
    }
}

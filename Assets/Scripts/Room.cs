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
    public List<Room> adjacentRooms;
    public List<Enemy> enemies;
    [SerializeField] Transform wallsParent;
    List<Wall> walls = new List<Wall>();
    RoomState state = RoomState.HIDDEN;
    public event System.Action<RoomState> onChangeState;
    List<PlayerUnit> scanningUnits = new List<PlayerUnit>();
    [System.NonSerialized] public List<Resource> resources = new List<Resource>();
    [SerializeField] bool alwaysPowered;
    public bool powered;

    private void Start()
    {
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

    public void ScanAdjacentRooms(PlayerUnit scanningUnit)
    {
        foreach(Room room in adjacentRooms)
        {
            room.GetScanned(scanningUnit);
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

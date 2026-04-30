using System.Collections.Generic;
using UnityEngine;

public enum RoomState
{
    HIDDEN, DISCOVERED, EXPLORED
}

public class Room : MonoBehaviour
{
    public List<Room> adjacentRooms;
    public List<Enemy> enemies;
    [SerializeField] GameObject wallsAndFloorObject;
    SpriteRenderer[] wallsAndFloor;
    RoomState state = RoomState.HIDDEN;
    public event System.Action<RoomState> onChangeState;
    [SerializeField] SpriteRenderer emptyIndicator;
    [SerializeField] SpriteRenderer enemyIndicator;
    List<PlayerUnit> scanningUnits = new List<PlayerUnit>();
    [System.NonSerialized] public List<Resource> resources = new List<Resource>();
    [SerializeField] bool alwaysPowered;
    public bool powered;

    private void Start()
    {
        wallsAndFloor = wallsAndFloorObject.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sprite in wallsAndFloor)
        {
            sprite.enabled = false;
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
                foreach(SpriteRenderer sprite in wallsAndFloor)
                {
                    sprite.enabled = true;
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
        if (!scanningUnits.Contains(scanningUnit))
        {
            scanningUnits.Add(scanningUnit);
        }
        if(enemies.Count == 0)
        {
            enemyIndicator.enabled = false;
            emptyIndicator.enabled = true;
        }
        else
        {
            enemyIndicator.enabled = true;
            emptyIndicator.enabled = false;
        }
    }

    void StopGettingScanned()
    {
        enemyIndicator.enabled = false;
        emptyIndicator.enabled = false;
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

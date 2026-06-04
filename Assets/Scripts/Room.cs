using System.Collections.Generic;
using UnityEngine;

public enum RoomState
{
    HIDDEN, DISCOVERED, EXPLORED
}

public enum ScanningState
{
    UNSCANNED, DANGER, SAFE, UNSCANNABLE
}

public class Room : MonoBehaviour
{
    [SerializeField] ColorData colorData;
    public List<Door> doors;
    public List<Enemy> enemies;
    [SerializeField] Transform wallsParent;
    List<Wall> walls = new List<Wall>();
    RoomState state = RoomState.HIDDEN;
    List<PlayerUnit> scanningUnits = new List<PlayerUnit>();
    [System.NonSerialized] public List<Resource> resources = new List<Resource>();
    [System.NonSerialized] public Terminal terminal;
    List<IPowerRooms> powerSources = new List<IPowerRooms>();
    BoxCollider2D boxCollider;
    public bool unscannable;
    List<ITakeDamage> damageTakers = new List<ITakeDamage>();
    [System.NonSerialized] public Portal portal = null;
    List<PlayerUnit> unitsInRoom = new List<PlayerUnit>();
    public float hellfire;
    bool hellfireDecay = true;
    [SerializeField] SpriteRenderer[] hellfireIcons;

    public event System.Action<RoomState> onChangeState;

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

        UpdateHellfireIcons();
        foreach(SpriteRenderer sprite in hellfireIcons)
        {
            sprite.color = colorData.danger;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state != RoomState.EXPLORED && collision.CompareTag("Player"))
        {
            SetState(RoomState.EXPLORED);
        }

        if(collision.TryGetComponent(out Enemy enteringEnemy))
        {
            enemies.Add(enteringEnemy);
        }

        if(collision.TryGetComponent(out ITakeDamage damageTaker))
        {
            damageTakers.Add(damageTaker);
        }

        if(collision.TryGetComponent(out PlayerUnit playerUnit))
        {
            unitsInRoom.Add(playerUnit);
            if(portal != null) playerUnit.UpdatePortalUi(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Enemy leavingEnemy))
        {
            enemies.Remove(leavingEnemy);
        }

        if(collision.TryGetComponent(out ITakeDamage damageTaker))
        {
            damageTakers.Remove(damageTaker);
        }

        if (collision.TryGetComponent(out PlayerUnit playerUnit))
        {
            unitsInRoom.Remove(playerUnit);
            if(portal != null) playerUnit.UpdatePortalUi(false);
        }
    }

    private void Update()
    {
        if(hellfireDecay && hellfire > 0)
        {
            hellfire -= 0.15f * Time.deltaTime;
            UpdateHellfireIcons();
        }
        hellfireDecay = true;

        if(hellfire > 1)
        {
            int hellfireInt = Mathf.FloorToInt(hellfire);
            List<Room> roomsToSpread = GetAccessibleRooms();
            roomsToSpread.Remove(this);
            foreach(Room room in roomsToSpread)
            {
                room.GainHellfire(hellfireInt);
            }
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

    public Vector3 GetRandomPointInRoom(float edgeBuffer = 0.3f)
    {
        float halfWidth = boxCollider.size.x / 2 - edgeBuffer;
        float halfHeight = boxCollider.size.y / 2 - edgeBuffer;
        float xOffset = Random.Range(-halfWidth, halfWidth);
        float yOffset = Random.Range(-halfHeight, halfHeight);
        return transform.position + new Vector3(xOffset, yOffset, 0);
    }

    public void AddPower(IPowerRooms powerRooms)
    {
        if(state == RoomState.HIDDEN)
        {
            SetState(RoomState.DISCOVERED);
        }
        powerSources.Add(powerRooms);
        foreach(Wall wall in walls)
        {
            wall.PowerChange(true);
        }
        foreach(Door door in doors)
        {
            door.UpdatePowerState();
        }
    }

    public void LosePower(IPowerRooms powerRooms)
    {
        powerSources.Remove(powerRooms);
        if(powerSources.Count == 0)
        {
            foreach (Wall wall in walls)
            {
                wall.PowerChange(false);
            }
        }
        foreach (Door door in doors)
        {
            door.UpdatePowerState();
        }
    }

    public bool HasPower()
    {
        return powerSources.Count > 0;
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

        if (unscannable)
        {
            SetWallScanState(ScanningState.UNSCANNABLE);
        }
        else if(enemies.Count == 0)
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
            wall.SetScanState(setState, powerSources.Count > 0);
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

    public void DamageRoom(int amount)
    {
        for(int i = damageTakers.Count -1; i >= 0; i--)
        {
            damageTakers[i].TakeDamage(amount);
        }
    }

    public void GainHellfire(int sourceLevel)
    {
        hellfireDecay = false;
        int hellfireInt = Mathf.FloorToInt(hellfire);
        if(sourceLevel == 3 && hellfireInt == 3 && hellfire < 3.5f)
        {
            hellfire += 0.2f * Time.deltaTime;
            UpdateHellfireIcons();
            return;
        }
        if (sourceLevel <= hellfireInt) return;
        int diff = sourceLevel - hellfireInt;
        hellfire += diff * 0.1f * Time.deltaTime;
        UpdateHellfireIcons();
    }

    void UpdateHellfireIcons()
    {
        for(int i = 0; i < hellfireIcons.Length; i++)
        {
            hellfireIcons[i].enabled = i + 1 < hellfire;
        }
    }

    public void AddDamageTaker(ITakeDamage damageTaker)
    {
        damageTakers.Add(damageTaker);
    }

    public void RemoveDamageTaker(ITakeDamage damageTaker)
    {
        damageTakers.Remove(damageTaker);
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

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
    public float dot;
    bool dotDecay = true;
    [SerializeField] SpriteRenderer[] hellfireIcons;
    [SerializeField] SpriteRenderer[] holyAuraIcons;
    float dotRate = 0.5f;
    float dotBuildup = 0;

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

        UpdateDotIcons();
        foreach(SpriteRenderer sprite in hellfireIcons)
        {
            sprite.color = colorData.danger;
        }
        foreach(SpriteRenderer sprite in holyAuraIcons)
        {
            sprite.color = colorData.holy;
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
        if(dotDecay && Mathf.Abs(dot) > 0.1f)
        {
            dot -= 0.3f * Mathf.Sign(dot) * Time.deltaTime;
            UpdateDotIcons();
        }
        dotDecay = true;

        if(dot < -1)
        {
            int hellfireInt = Mathf.CeilToInt(dot);
            List<Room> roomsToSpread = GetAccessibleRooms();
            roomsToSpread.Remove(this);
            foreach(Room room in roomsToSpread)
            {
                room.GainHellfire(hellfireInt);
            }

            dotBuildup += Time.deltaTime;
            if(dotBuildup >= dotRate)
            {
                for(int i = unitsInRoom.Count - 1; i >= 0; i--)
                {
                    unitsInRoom[i].TakeHellfireDamage(hellfireInt);
                }
                dotBuildup = 0;
            }
        }
        else if(dot > 1)
        {
            int holyAuraInt = Mathf.FloorToInt(dot);
            List<Room> roomsToSpread = GetAccessibleRooms();
            roomsToSpread.Remove(this);
            foreach (Room room in roomsToSpread)
            {
                room.GainHolyAura(holyAuraInt);
            }

            dotBuildup += Time.deltaTime;
            if (dotBuildup >= dotRate)
            {
                for (int i = unitsInRoom.Count - 1; i >= 0; i--)
                {
                    unitsInRoom[i].TakeHolyAuraDamage(holyAuraInt);
                }
                dotBuildup = 0;
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
        dotDecay = false;
        int hellfireInt = Mathf.CeilToInt(dot);
        if(sourceLevel == -3 && hellfireInt == -3 && dot > -3.5f)
        {
            dot -= 0.2f * Time.deltaTime;
            UpdateDotIcons();
            return;
        }
        if (sourceLevel >= hellfireInt) return;
        int diff = sourceLevel - hellfireInt;
        dot += diff * 0.1f * Time.deltaTime;
        UpdateDotIcons();
    }

    public void GainHolyAura(int sourceLevel)
    {
        dotDecay = false;
        int auraInt = Mathf.FloorToInt(dot);
        if(sourceLevel == 3 && auraInt == 3 && dot < 3.5f)
        {
            dot += 0.2f * Time.deltaTime;
            UpdateDotIcons();
            return;
        }
        if (sourceLevel <= auraInt) return;
        int diff = sourceLevel - auraInt;
        dot += diff * 0.1f * Time.deltaTime;
        UpdateDotIcons();
    }

    void UpdateDotIcons()
    {
        for(int i = 0; i < hellfireIcons.Length; i++)
        {
            hellfireIcons[i].enabled = -i - 1 > dot;
        }
        for(int i = 0; i < holyAuraIcons.Length; i++)
        {
            holyAuraIcons[i].enabled = i + 1 < dot;
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

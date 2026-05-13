using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager i;

    [System.NonSerialized] public PlayerUnit[] allUnits = new PlayerUnit[4];
    [System.NonSerialized] public List<PlayerUnit> selectedUnits = new List<PlayerUnit>();
    int resources = 0;

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;
    }

    void LeftClick(Vector3 worldPos)
    {
        DeselectAll();

        Collider2D[] hits = Physics2D.OverlapPointAll(worldPos, Layers.clickableMask);
        foreach(Collider2D hit in hits)
        {
            if (hit != null && hit.CompareTag("Player"))
            {
                PlayerUnit playerUnit = hit.GetComponentInParent<PlayerUnit>();
                SelectUnit(playerUnit);
            }
        }
    }

    public void RightClick(Vector3 worldPos)
    {
        foreach (PlayerUnit unit in selectedUnits)
        {
            unit.SetDestination(worldPos);
        }
    }

    private void Ability(int index)
    {
        if (selectedUnits.Count != 1) return;
        selectedUnits[0].PerformAbility(index);
    }

    public void Portal()
    {
        if (selectedUnits.Count != 1) return;
        selectedUnits[0].unitAbilities.InteractWithPortal();
    }

    public void SelectButton(int unitIndex)
    {
        DeselectAll();
        if(unitIndex < allUnits.Length)
        {
            SelectUnit(allUnits[unitIndex]);
        }
    }

    void SelectUnit(PlayerUnit unit)
    {
        selectedUnits.Add(unit);
        unit.SetSelected(true);
        GlobalEvents.i.SelectUnits(selectedUnits);
    }

    void DeselectAll()
    {
        selectedUnits.Clear();
        GlobalEvents.i.DeselectAll();
    }

    private void PlayerEvents_onUnitExists(PlayerUnit unit)
    {
        allUnits[unit.data.index] = unit;
    }

    public void GainResources(int amount)
    {
        resources += amount;
        GlobalEvents.i.UpdateResources(resources);
    }

    private void Player_onUnitLeaveMission(PlayerUnit leftUnit)
    {
        selectedUnits.Remove(leftUnit);
        allUnits[leftUnit.data.index] = null;
        PlayerEvents.i.UnitStatChange(leftUnit);
        GlobalEvents.i.UnitLeaveMission(leftUnit);
    }

    private void PlayerEvents_onUnitDeath(PlayerUnit deadUnit)
    {
        selectedUnits.Remove(deadUnit);
        allUnits[deadUnit.data.index] = null;
        PlayerEvents.i.UnitStatChange(deadUnit);
    }

    private void Player_onUnitStatChange(PlayerUnit playerUnit)
    {
        GlobalEvents.i.UnitStatChange(playerUnit, selectedUnits.Count == 1 && selectedUnits[0] == playerUnit);

    }

    private void Player_onPortalRoomChange(PlayerUnit playerUnit, bool inPortalRoom)
    {
        if (selectedUnits.Count != 1 || selectedUnits[0] != playerUnit) return;
        GlobalEvents.i.PortalRoomChange(playerUnit, inPortalRoom);
    }

    void OnEnable()
    {
        InputManager.i.SetControlUnits();
        InputManager.i.onLeftClick += LeftClick;
        InputManager.i.onRightClick += RightClick;
        InputManager.i.onAbility += Ability;
        InputManager.i.onPortal += Portal;
        InputManager.i.onSelectButton += SelectButton;
        PlayerEvents.i.onUnitExists += PlayerEvents_onUnitExists;
        PlayerEvents.i.onUnitDeath += PlayerEvents_onUnitDeath;
        PlayerEvents.i.onUnitStatChange += Player_onUnitStatChange;
        PlayerEvents.i.onPortalRoomChange += Player_onPortalRoomChange;
        PlayerEvents.i.onUnitLeaveMission += Player_onUnitLeaveMission;
    }

    private void OnDisable()
    {
        InputManager.i.DisableControlUnits();
        InputManager.i.onLeftClick -= LeftClick;
        InputManager.i.onRightClick -= RightClick;
        InputManager.i.onAbility -= Ability;
        InputManager.i.onPortal -= Portal;
        InputManager.i.onSelectButton -= SelectButton;
        PlayerEvents.i.onUnitExists -= PlayerEvents_onUnitExists;
        PlayerEvents.i.onUnitDeath -= PlayerEvents_onUnitDeath;
        PlayerEvents.i.onUnitStatChange -= Player_onUnitStatChange;
        PlayerEvents.i.onPortalRoomChange -= Player_onPortalRoomChange;
        PlayerEvents.i.onUnitLeaveMission -= Player_onUnitLeaveMission;
    }
}

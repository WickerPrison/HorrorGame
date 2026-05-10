using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [System.NonSerialized] public List<PlayerUnit> allUnits = new List<PlayerUnit>();
    [System.NonSerialized] public List<PlayerUnit> selectedUnits = new List<PlayerUnit>();
    int resources = 0;

    void LeftClick(Vector3 worldPos)
    {
        DeselectAll();

        Collider2D hit = Physics2D.OverlapPoint(worldPos, Layers.clickableMask);
        if (hit != null && hit.CompareTag("Player"))
        {
            PlayerUnit playerUnit = hit.GetComponentInParent<PlayerUnit>();
            SelectUnit(playerUnit);
        }
    }

    void RightClick(Vector3 worldPos)
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

    private void SelectButton(int unitIndex)
    {
        DeselectAll();
        if(unitIndex < allUnits.Count)
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
        allUnits.Add(unit);
    }

    public void GainResources(int amount)
    {
        resources += amount;
        GlobalEvents.i.UpdateResources(resources);
    }

    private void PlayerEvents_onUnitDeath(PlayerUnit deadUnit)
    {
        selectedUnits.Remove(deadUnit);
        allUnits.Remove(deadUnit);
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
        InputManager.i.onSelectButton += SelectButton;
        PlayerEvents.i.onUnitExists += PlayerEvents_onUnitExists;
        PlayerEvents.i.onUnitDeath += PlayerEvents_onUnitDeath;
        PlayerEvents.i.onUnitStatChange += Player_onUnitStatChange;
        PlayerEvents.i.onPortalRoomChange += Player_onPortalRoomChange;
    }

    private void OnDisable()
    {
        InputManager.i.DisableControlUnits();
        InputManager.i.onLeftClick -= LeftClick;
        InputManager.i.onRightClick -= RightClick;
        InputManager.i.onAbility -= Ability;
        InputManager.i.onSelectButton -= SelectButton;
        PlayerEvents.i.onUnitExists -= PlayerEvents_onUnitExists;
        PlayerEvents.i.onUnitDeath -= PlayerEvents_onUnitDeath;
        PlayerEvents.i.onUnitStatChange -= Player_onUnitStatChange;
        PlayerEvents.i.onPortalRoomChange -= Player_onPortalRoomChange;
    }
}

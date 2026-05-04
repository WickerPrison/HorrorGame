using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [System.NonSerialized] public List<PlayerUnit> allUnits = new List<PlayerUnit>();
    List<PlayerUnit> selectedUnits = new List<PlayerUnit>();
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

    private void Scan()
    {
        foreach(PlayerUnit unit in selectedUnits)
        {
            unit.Scan();
        }
    }

    private void Collect()
    {
        foreach(PlayerUnit unit in selectedUnits)
        {
            unit.Collect();
        }
    }

    private void Hack()
    {
        foreach(PlayerUnit unit in selectedUnits)
        {
            unit.Hack();
        }
    }

    void SelectUnit(PlayerUnit unit)
    {
        selectedUnits.Add(unit);
        unit.SetSelected(true);
    }

    void DeselectAll()
    {
        selectedUnits.Clear();
        PlayerEvents.i.DeselectAll();
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
    }

    void OnEnable()
    {
        InputManager.i.SetControlUnits();
        InputManager.i.onLeftClick += LeftClick;
        InputManager.i.onRightClick += RightClick;
        InputManager.i.onScan += Scan;
        InputManager.i.onCollect += Collect; 
        InputManager.i.onHack += Hack;
        PlayerEvents.i.onUnitExists += PlayerEvents_onUnitExists;
        PlayerEvents.i.onUnitDeath += PlayerEvents_onUnitDeath;
    }

    private void OnDisable()
    {
        InputManager.i.DisableControlUnits();
        InputManager.i.onLeftClick -= LeftClick;
        InputManager.i.onRightClick -= RightClick;
        InputManager.i.onScan -= Scan;
        InputManager.i.onCollect -= Collect;
        InputManager.i.onHack -= Hack;
        PlayerEvents.i.onUnitExists -= PlayerEvents_onUnitExists;
        PlayerEvents.i.onUnitDeath -= PlayerEvents_onUnitDeath;
    }
}

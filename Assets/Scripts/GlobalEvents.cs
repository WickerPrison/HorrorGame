using System;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEvents : MonoBehaviour
{
    public static GlobalEvents i;

    public event Action<PlayerUnit> onUnitStopScanning;
    public event Action onUpdateResources;
    public event Action<List<PlayerUnit>> onSelectUnits;
    public event EventHandler onDeselectAll;
    public event Action<Enemy> onEnemyDeath;
    public event Action<PlayerUnit, bool> onUnitStatChange;
    public event Action<PlayerUnit, bool> onPortalRoomChange;
    public event Action<bool> onPortalActivation;
    public event Action<PlayerUnit> onUnitLeaveMission;

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;
    }

    public void UnitStopScanning(PlayerUnit scanningUnit)
    {
        onUnitStopScanning?.Invoke(scanningUnit);
    }

    public void UpdateResources()
    {
        onUpdateResources?.Invoke();
    }

    public void SelectUnits(List<PlayerUnit> selectedUnits)
    {
        onSelectUnits?.Invoke(selectedUnits);
    }

    public void DeselectAll()
    {
        onDeselectAll?.Invoke(this, EventArgs.Empty);
    }

    public void UnitStatChange(PlayerUnit playerUnit, bool isOnlySelectedUnit)
    {
        onUnitStatChange?.Invoke(playerUnit, isOnlySelectedUnit);
    }

    public void PortalRoomChange(PlayerUnit playerUnit, bool inPortalRoom)
    {
        onPortalRoomChange?.Invoke(playerUnit, inPortalRoom);
    }

    public void PortalActivation(bool activated)
    {
        onPortalActivation?.Invoke(activated);
    }

    public void UnitLeaveMission(PlayerUnit unit)
    {
        onUnitLeaveMission?.Invoke(unit);
    }
}

using System;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    public static PlayerEvents i;

    public event System.Action<PlayerUnit> onUnitExists;
    public event Action<PlayerUnit> onUnitDeath;
    public event Action<PlayerUnit> onUnitStatChange;
    public event Action<PlayerUnit, bool> onPortalRoomChange;

    private void Awake()
    {
        if(i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;
    }

    public void UnitExists(PlayerUnit unit)
    {
        onUnitExists?.Invoke(unit);
    }

    public void UnitDeath(PlayerUnit unit)
    {
        onUnitDeath?.Invoke(unit);
    }

    public void UnitStatChange(PlayerUnit changedUnit)
    {
        onUnitStatChange?.Invoke(changedUnit);
    }

    public void PortalRoomChange(PlayerUnit playerUnit, bool inPortalRoom)
    {
        onPortalRoomChange?.Invoke(playerUnit, inPortalRoom);
    }
}

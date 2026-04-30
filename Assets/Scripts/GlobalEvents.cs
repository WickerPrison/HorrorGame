using System;
using UnityEngine;

public class GlobalEvents : MonoBehaviour
{
    public static GlobalEvents i;

    public event Action<PlayerUnit> onUnitStopScanning;
    public event Action<int> onUpdateResources;

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

    public void UpdateResources(int amount)
    {
        onUpdateResources?.Invoke(amount);
    }
}

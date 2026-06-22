using System;
using UnityEngine;

public class CampaignEvents : MonoBehaviour
{
    public static CampaignEvents i;

    public event Action onUpdateSquad;
    public event Action<PlayerUnitData> onSelectUnit;
    public event Action onUpdateAbilities;

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;
    }

    public void UpdateSquad()
    {
        onUpdateSquad?.Invoke();
    }

    public void SelectUnit(PlayerUnitData unitData)
    {
        onSelectUnit?.Invoke(unitData);
    }

    public void UpdateAbilities()
    {
        onUpdateAbilities?.Invoke();
    }
}

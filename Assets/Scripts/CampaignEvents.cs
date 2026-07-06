using System;
using UnityEngine;

public class CampaignEvents : MonoBehaviour
{
    public static CampaignEvents i;

    public event Action onUpdateSquad;
    public event Action<PlayerUnitData> onSelectUnit;
    public event Action onUpdateAbilities;
    public event Action<string> onSetDescription;
    public event Action onUpdateResources;

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

    public void SetDescription(string description)
    {
        onSetDescription?.Invoke(description);
    }

    public void UpdateResources()
    {
        onUpdateResources?.Invoke();
    }
}

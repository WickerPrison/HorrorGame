using System.Collections.Generic;
using UnityEngine;

public class AbilitiesUi : MonoBehaviour
{
    [SerializeField] List<AbilityIcon> abilityIcons;

    private void Global_onSelectUnits(List<PlayerUnit> selectedUnits)
    {
        if(selectedUnits.Count == 1)
        {
            SetIconsForUnit(selectedUnits[0]);
        }
        else
        {
            HideIcons();
        }
    }

    private void Global_onDeselectAll(object sender, System.EventArgs e)
    {
        HideIcons();
    }

    void HideIcons()
    {
        foreach (AbilityIcon icon in abilityIcons)
        {
            icon.SetAbility(Ability.NONE, null);
        }
    }

    void SetIconsForUnit(PlayerUnit playerUnit)
    {
        for (int i = 0; i < 4; i++)
        {
            abilityIcons[i].SetAbility(playerUnit.data.abilities[i], playerUnit.data);
        }
    }

    private void Player_onUnitStatChange(PlayerUnit playerUnit)
    {
        SetIconsForUnit(playerUnit);
    }

    private void OnEnable()
    {
        GlobalEvents.i.onSelectUnits += Global_onSelectUnits;
        GlobalEvents.i.onDeselectAll += Global_onDeselectAll;
        PlayerEvents.i.onUnitStatChange += Player_onUnitStatChange;
    }

    private void OnDisable()
    {
        GlobalEvents.i.onSelectUnits -= Global_onSelectUnits;
        GlobalEvents.i.onDeselectAll -= Global_onDeselectAll;
        PlayerEvents.i.onUnitStatChange -= Player_onUnitStatChange;
    }
}

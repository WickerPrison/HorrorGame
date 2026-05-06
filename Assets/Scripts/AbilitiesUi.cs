using System.Collections.Generic;
using UnityEngine;

public class AbilitiesUi : MonoBehaviour
{
    [SerializeField] List<AbilityIcon> abilityIcons;

    private void Global_onSelectUnits(List<PlayerUnit> selectedUnits)
    {
        if(selectedUnits.Count == 1)
        {
            for(int i = 0; i < 4; i++)
            {
                abilityIcons[i].SetAbility(selectedUnits[0].data.abilities[i]);
            }
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
            icon.SetAbility(Ability.NONE);
        }
    }

    private void OnEnable()
    {
        GlobalEvents.i.onSelectUnits += Global_onSelectUnits;
        GlobalEvents.i.onDeselectAll += Global_onDeselectAll;
    }

    private void OnDisable()
    {
        GlobalEvents.i.onSelectUnits -= Global_onSelectUnits;
        GlobalEvents.i.onDeselectAll -= Global_onDeselectAll;
    }
}

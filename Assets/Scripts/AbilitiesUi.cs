using System.Collections.Generic;
using UnityEngine;

public class AbilitiesUi : MonoBehaviour
{
    [SerializeField] AbilityIcon portalIcon;
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
            icon.Show(false);
        }
        portalIcon.Show(false);
    }

    void SetIconsForUnit(PlayerUnit playerUnit)
    {
        for (int i = 0; i < 4; i++)
        {
            abilityIcons[i].SetAbility(playerUnit.data.abilities[i]);
        }
        Room room = Utils.GetRoom(playerUnit.transform.position, 0.1f);
        bool unitInRoomWithPortal = room != null && room.portal != null;
        portalIcon.Show(unitInRoomWithPortal);
    }

    private void Global_onUnitStatChange(PlayerUnit playerUnit, bool isOnlySelectedUnit)
    {
        if (!isOnlySelectedUnit) return;
        SetIconsForUnit(playerUnit);
    }

    private void Global_onPortalRoomChange(PlayerUnit playerUnit, bool inPortalRoom)
    {
        portalIcon.Show(inPortalRoom);
    }

    private void Global_onPortalActivation(bool activated)
    {
        if (activated)
        {
            portalIcon.SetTexts("Exit", "X");
        }
        else
        {
            portalIcon.SetTexts("Portal", "F");
        }
    }

    private void OnEnable()
    {
        MissionEvents.i.onSelectUnits += Global_onSelectUnits;
        MissionEvents.i.onDeselectAll += Global_onDeselectAll;
        MissionEvents.i.onUnitStatChange += Global_onUnitStatChange;
        MissionEvents.i.onPortalRoomChange += Global_onPortalRoomChange;
        MissionEvents.i.onPortalActivation += Global_onPortalActivation;
    }

    private void OnDisable()
    {
        MissionEvents.i.onSelectUnits -= Global_onSelectUnits;
        MissionEvents.i.onDeselectAll -= Global_onDeselectAll;
        MissionEvents.i.onUnitStatChange -= Global_onUnitStatChange;
        MissionEvents.i.onPortalRoomChange -= Global_onPortalRoomChange;
        MissionEvents.i.onPortalActivation -= Global_onPortalActivation;
    }
}

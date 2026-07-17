using UnityEngine;
using UnityEngine.UI;

public class SelectedUnit : MonoBehaviour
{
    Image outline;
    UnitStatUi unitStatUi;
    [SerializeField] AbilityControls[] abilityControls;
    PlayerUnitData selectedUnit;
    [SerializeField] CampaignData campaignData;

    void Start()
    {
        outline = GetComponent<Image>();
        unitStatUi = GetComponentInChildren<UnitStatUi>();
        SetSelectedUnit(null);
    }

    void SetSelectedUnit(PlayerUnitData unitData)
    {
        outline.enabled = unitData != null;
        unitStatUi.SetUnitWithAbilityDescription(unitData, 16);
        foreach(AbilityControls controls in abilityControls)
        {
            controls.SetUnitData(unitData);
        }
        selectedUnit = unitData;
    }

    void RefreshUnitDisplay()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Campaign_onSelectUnit(PlayerUnitData unitData)
    {
        SetSelectedUnit(unitData);
    }

    public void MoveAbilityUp(int index)
    {
        (selectedUnit.abilities[index], selectedUnit.abilities[index - 1]) = (selectedUnit.abilities[index - 1], selectedUnit.abilities[index]);
        RefreshUnitDisplay();
        CampaignEvents.i.UpdateSquad();
    }

    public void MoveAbilityDown(int index)
    {
        (selectedUnit.abilities[index], selectedUnit.abilities[index + 1]) = (selectedUnit.abilities[index + 1], selectedUnit.abilities[index]);
        RefreshUnitDisplay();
        CampaignEvents.i.UpdateSquad();
    }

    public void UnequipAbility(int index)
    {
        campaignData.unequippedAbilities.Add(selectedUnit.abilities[index]);
        selectedUnit.abilities[index] = Ability.None();
        CampaignEvents.i.SelectUnit(selectedUnit);
        CampaignEvents.i.UpdateSquad();
    }

    private void Campaign_onUpdateAbilities()
    {
        if (selectedUnit == null) return;
        SetSelectedUnit(selectedUnit);
    }

    private void OnEnable()
    {
        CampaignEvents.i.onSelectUnit += Campaign_onSelectUnit;
        CampaignEvents.i.onUpdateAbilities += Campaign_onUpdateAbilities;
    }

    private void OnDisable()
    {
        CampaignEvents.i.onSelectUnit -= Campaign_onSelectUnit;
        CampaignEvents.i.onUpdateAbilities -= Campaign_onUpdateAbilities;
    }
}

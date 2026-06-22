using System.Collections.Generic;
using UnityEngine;

public class AbilityBank : MonoBehaviour
{
    [SerializeField] CampaignData campaignData;
    [SerializeField] GameObject abilityPrefab;
    List<AbilityBankItem> abilityBankItems = new List<AbilityBankItem>();
    PlayerUnitData selectedUnit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetAbilities();
    }

    public void SetAbilities()
    {
        for(int i = 0; i < Mathf.Max(campaignData.unequippedAbilities.Count, abilityBankItems.Count); i++)
        {
            if(i < campaignData.unequippedAbilities.Count)
            {
                if(i < abilityBankItems.Count)
                {
                    abilityBankItems[i].SetAbility(campaignData.unequippedAbilities[i], selectedUnit);
                }
                else
                {
                    AbilityBankItem newBankItem = Instantiate(abilityPrefab).GetComponent<AbilityBankItem>();
                    newBankItem.SetAbility(campaignData.unequippedAbilities[i], selectedUnit);
                    newBankItem.transform.SetParent(transform);
                    newBankItem.abilityBank = this;
                    abilityBankItems.Add(newBankItem);
                }
            }
            else
            {
                abilityBankItems[i].SetAbility(null, selectedUnit);
            }
        }
    }

    public void EquipAbility(Ability ability)
    {
        if (selectedUnit == null) return;
        for(int i = 0; i < selectedUnit.abilities.Length; i++)
        {
            if(selectedUnit.abilities[i].type == AbilityType.NONE)
            {
                selectedUnit.abilities[i] = ability;
                break;
            }
        }
        campaignData.unequippedAbilities.Remove(ability);
        CampaignEvents.i.UpdateAbilities();
        CampaignEvents.i.UpdateSquad();
    }

    private void Campaign_onUpdateAbilities()
    {
        SetAbilities();
    }

    private void Campaign_onSelectUnit(PlayerUnitData unit)
    {
        selectedUnit = unit;
        SetAbilities();
    }

    private void OnEnable()
    {
        CampaignEvents.i.onUpdateAbilities += Campaign_onUpdateAbilities;
        CampaignEvents.i.onSelectUnit += Campaign_onSelectUnit;
    }

    private void OnDisable()
    {
        CampaignEvents.i.onUpdateAbilities -= Campaign_onUpdateAbilities;
        CampaignEvents.i.onSelectUnit -= Campaign_onSelectUnit;
    }
}

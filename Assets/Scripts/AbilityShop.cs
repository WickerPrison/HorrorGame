using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityShop : MonoBehaviour
{
    [SerializeField] GameObject abilityShopItemPrefab;
    [SerializeField] AbilityDictionary abilityDictionary;
    List<AbilityShopItem> shopItems;
    [SerializeField] CampaignData campaignData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shopItems = GetComponentsInChildren<AbilityShopItem>().ToList();
        UpdateStock();
    }

    void UpdateStock()
    {
        shopItems[0].SetAbility(new Ability(abilityDictionary, AbilityType.COLLECT));
        shopItems[1].SetAbility(new Ability(abilityDictionary, AbilityType.SCAN));
        shopItems[2].SetAbility(new Ability(abilityDictionary, AbilityType.POWER));
    }

    public void BuyAbility(Ability ability)
    {
        if (campaignData.resources < ability.cost) return;
        campaignData.resources -= ability.cost;
        campaignData.unequippedAbilities.Add(ability);
        UpdateStock();
        CampaignEvents.i.UpdateAbilities();
        CampaignEvents.i.UpdateResources();
    }
}

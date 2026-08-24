using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbilityShop : MonoBehaviour
{
    [SerializeField] GameObject abilityShopItemPrefab;
    [SerializeField] AbilityDictionary abilityDictionary;
    AbilityShopItem[] permanentStock;
    List<AbilityShopItem> shopItems = new List<AbilityShopItem>();
    [SerializeField] CampaignData campaignData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        permanentStock = GetComponentsInChildren<AbilityShopItem>();
        UpdateStock();
    }

    void UpdateStock()
    {
        permanentStock[0].SetAbility(new Ability(abilityDictionary, AbilityType.COLLECT));
        permanentStock[1].SetAbility(new Ability(abilityDictionary, AbilityType.SCAN));
        permanentStock[2].SetAbility(new Ability(abilityDictionary, AbilityType.POWER));
        SetAbilities();
    }

    public void SetAbilities()
    {
        for (int i = 0; i < Mathf.Max(campaignData.shopInventory.Count, shopItems.Count); i++)
        {
            if (i < campaignData.shopInventory.Count)
            {
                if (i < shopItems.Count)
                {
                    shopItems[i].SetAbility(campaignData.shopInventory[i]);
                }
                else
                {
                    AbilityShopItem newShopItem = Instantiate(abilityShopItemPrefab).GetComponent<AbilityShopItem>();
                    newShopItem.SetAbility(campaignData.shopInventory[i]);
                    newShopItem.transform.SetParent(transform);
                    newShopItem.transform.localScale = Vector3.one;
                    newShopItem.abilityShop = this;
                    shopItems.Add(newShopItem);
                }
            }
            else
            {
                shopItems[i].SetAbility(null);
            }
        }
    }

    public void BuyAbility(Ability ability)
    {
        if (campaignData.aether < ability.cost) return;
        campaignData.shopInventory.Remove(ability);
        BuyAbilityDontRemove(ability);
    }

    public void BuyAbilityDontRemove(Ability ability)
    {
        if (campaignData.aether < ability.cost) return;
        campaignData.aether -= ability.cost;
        campaignData.unequippedAbilities.Add(ability);
        UpdateStock();
        CampaignEvents.i.UpdateAbilities();
        CampaignEvents.i.UpdateResources();
    }
}

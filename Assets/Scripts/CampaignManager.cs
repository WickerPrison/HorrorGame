using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CampaignManager : MonoBehaviour
{
    public static CampaignManager i;
    [SerializeField] CampaignData campaignData;
    [SerializeField] GameObject barracks;
    [SerializeField] GameObject selectedUnit;
    [SerializeField] GameObject abilities;
    [SerializeField] GameObject recruits;
    [SerializeField] GameObject missionList;
    [SerializeField] MenuButton manageUnits;
    [SerializeField] MenuButton missionSelect;
    [SerializeField] List<AbilityData> abilitiesForShop = new List<AbilityData>();
    [SerializeField] AbilityDictionary abilityDictionary;

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;

        GenerateRecruits();
        GenerateShopInventory();
    }

    private void Start()
    {
        ManageUnits();
    }

    public void MissionSelect()
    {
        barracks.SetActive(false);
        selectedUnit.SetActive(false);
        abilities.SetActive(false);
        recruits.SetActive(false);
        missionList.SetActive(true);
        missionSelect.interactable = false;
        manageUnits.interactable = true;
    }

    public void ManageUnits()
    {
        barracks.SetActive(true);
        selectedUnit.SetActive(true);
        abilities.SetActive(true);
        recruits.SetActive(true);
        missionList.SetActive(false);
        missionSelect.interactable = true;
        manageUnits.interactable = false;
        CampaignEvents.i.UpdateSquad();
    }

    void GenerateRecruits()
    {
        campaignData.recruits = new List<PlayerUnitData>();
        for(int i = 0; i < 4; i++)
        {
            string name = $"Unit {Utils.GetRandomAlphanumericString(5)}";
            int randInt = UnityEngine.Random.Range(0, 20);
            int randInt2 = UnityEngine.Random.Range(0, 20);
            int health = 70 + randInt + randInt2;
            randInt = UnityEngine.Random.Range(-5, 6);
            randInt2 = UnityEngine.Random.Range(-5, 6);
            int morality = randInt + randInt2;
            PlayerUnitData newUnit = new PlayerUnitData(name, health, 0, morality);
            newUnit.cost = UnityEngine.Random.Range(0, 4) + 3;
            campaignData.recruits.Add(newUnit);
        }
    }

    void GenerateShopInventory()
    {
        campaignData.shopInventory.Clear();
        List<AbilityType> abilityOptions = abilityDictionary.commonAbilities.ToList();
        for (int i = 0; i < 3; i++)
        {
            int randInt = UnityEngine.Random.Range(0, abilityOptions.Count);
            campaignData.shopInventory.Add(new Ability(abilityDictionary, abilityOptions[randInt]));
            abilityOptions.RemoveAt(randInt);
        }
    }

    public bool CanAfford(int cost)
    {
        return cost <= campaignData.aether;
    }

    public void SpendResources(int amount)
    {
        campaignData.aether -= amount;
        CampaignEvents.i.UpdateResources();
    }
}

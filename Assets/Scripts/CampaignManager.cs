using System;
using System.Collections.Generic;
using UnityEngine;


public class CampaignManager : MonoBehaviour
{
    public static CampaignManager i;
    [SerializeField] CampaignData campaignData;
    [SerializeField] GameObject barracks;
    [SerializeField] GameObject selectedUnit;
    [SerializeField] GameObject abilities;
    [SerializeField] GameObject recruits;
    [SerializeField] MenuButton manageUnits;
    [SerializeField] MenuButton shop;
    [SerializeField] List<AbilityData> abilitiesForShop = new List<AbilityData>();

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;

        GenerateRecruits();
    }

    private void Start()
    {
        //ManageUnits();
    }

    public void Shop()
    {
        barracks.SetActive(false);
        selectedUnit.SetActive(false);
        abilities.SetActive(false);
        recruits.SetActive(true);
        shop.interactable = false;
        manageUnits.interactable = true;
    }

    public void ManageUnits()
    {
        barracks.SetActive(true);
        selectedUnit.SetActive(true);
        abilities.SetActive(true);
        recruits.SetActive(false);
        shop.interactable = true;
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
            newUnit.cost = UnityEngine.Random.Range(0, 5) + 6;
            campaignData.recruits.Add(newUnit);
        }
    }

    void GenerateShopInventory()
    {

    }

    public bool CanAfford(int cost)
    {
        return cost <= campaignData.resources;
    }

    public void SpendResources(int amount)
    {
        campaignData.resources -= amount;
        CampaignEvents.i.UpdateResources();
    }
}

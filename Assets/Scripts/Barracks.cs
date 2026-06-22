using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Barracks : MonoBehaviour
{
    [SerializeField] CampaignData campaignData;
    List<PlayerUnitData> unassignedUnits;
    [SerializeField] BarracksUnit[] barracksUnits;
    [SerializeField] SquadMenu squadMenu;

    private void Start()
    {
        UpdateUnits();
    }

    void UpdateUnits()
    {
        bool squadHasRoom = campaignData.squad.Any(unit => unit == null);
        unassignedUnits = campaignData.playerUnits.Where(unit => !campaignData.squad.Contains(unit)).ToList();
        for(int i = 0; i < barracksUnits.Length; i++)
        {
            if(i < unassignedUnits.Count)
            {
                barracksUnits[i].SetUnitData(unassignedUnits[i], squadHasRoom);
            }
            else
            {
                barracksUnits[i].SetUnitData(null, squadHasRoom);
            }
        }
    }

    private void Campaign_onUpdateSquad()
    {
        UpdateUnits();
    }

    public void AssignUnit(PlayerUnitData unitData)
    {
        squadMenu.AssignToSquad(unitData);
    }

    private void OnEnable()
    {
        CampaignEvents.i.onUpdateSquad += Campaign_onUpdateSquad;
    }

    private void OnDisable()
    {
        CampaignEvents.i.onUpdateSquad -= Campaign_onUpdateSquad;
    }
}

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Barracks : MonoBehaviour
{
    [SerializeField] CampaignData campaignData;
    List<PlayerUnitData> unassignedUnits;
    [SerializeField] BarracksUnit[] barracksUnits;

    void Start()
    {
        UpdateUnits();
    }

    void UpdateUnits()
    {
        unassignedUnits = campaignData.playerUnits.Where(unit => !campaignData.squad.Contains(unit)).ToList();
        for(int i = 0; i < barracksUnits.Length; i++)
        {
            if(i < unassignedUnits.Count)
            {
                barracksUnits[i].SetUnitData(unassignedUnits[i]);
            }
            else
            {
                barracksUnits[i].SetUnitData(null);
            }
        }
    }

    private void Campaign_onUpdateSquad()
    {
        UpdateUnits();
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

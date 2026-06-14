using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CampaignData", menuName = "Scriptable Objects/CampaignData")]
public class CampaignData : ScriptableObject
{
    [SerializeField] TestPlayerUnitData startingUnit1;
    [SerializeField] TestPlayerUnitData startingUnit2;
    [SerializeField] AbilityDictionary abilityDictionary;

    public List<LevelDetailsData> missions;
    public List<PlayerUnitData> playerUnits;
    public int resources;

    public void ResetCampaignData()
    {
        missions = new List<LevelDetailsData>() { new LevelDetailsData(), new LevelDetailsData(), new LevelDetailsData(), new LevelDetailsData(), new LevelDetailsData() };
        resources = 10;
        playerUnits = new List<PlayerUnitData>() { new PlayerUnitData(abilityDictionary, startingUnit1), new PlayerUnitData(abilityDictionary, startingUnit2) };
    }
}

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
    public PlayerUnitData[] squad = new PlayerUnitData[4];
    public List<PlayerUnitData> recruits = new List<PlayerUnitData>();
    public int aether;
    public List<Ability> unequippedAbilities;
    public bool testingLevel = false;
    public List<Ability> shopInventory = new List<Ability>();

    public void ResetCampaignData()
    {
        missions = new List<LevelDetailsData>();
        for(int i = 0; i < 5; i++)
        {
            LevelDetailsData newMission = new LevelDetailsData();
            newMission.GenerateData();
            missions.Add(newMission);
        }
        aether = 10;
        playerUnits = new List<PlayerUnitData>() { new PlayerUnitData(abilityDictionary, startingUnit1), new PlayerUnitData(abilityDictionary, startingUnit2) };
        squad[0] = playerUnits[0];
        squad[1] = playerUnits[1];
        squad[2] = new PlayerUnitData();
        squad[3] = new PlayerUnitData();
        unequippedAbilities = new List<Ability>();
    }
}

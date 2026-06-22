using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CampaignMenuTests
{
    CampaignData campaignData;
    TestingData testData;
    AbilityDictionary abilityDictionary;

    [SetUp]
    public void Setup()
    {
        testData = Resources.Load<TestingData>("Data/TestingData");
        campaignData = Resources.Load<CampaignData>("Data/CampaignData");
        abilityDictionary = Resources.Load<AbilityDictionary>("Data/Abilities/AbilityDictionary");
        Time.timeScale = testData.timeScale;
        campaignData.missions = null;
        SceneManager.LoadScene("MissionSelect");
    }

    PlayerUnitData CreateTestDummy()
    {
        return new PlayerUnitData("Test Dummy", 100, 0);
    }

    Ability CreateNewAbility(AbilityType type = AbilityType.SCAN)
    {
        return new Ability(abilityDictionary, type);
    }

    [UnityTest]
    public IEnumerator FullBarracksTest()
    {
        campaignData.playerUnits.Add(CreateTestDummy());
        campaignData.playerUnits.Add(CreateTestDummy());
        campaignData.playerUnits.Add(CreateTestDummy());
        campaignData.playerUnits.Add(CreateTestDummy());
        campaignData.playerUnits.Add(CreateTestDummy());
        campaignData.playerUnits.Add(CreateTestDummy());
        CampaignEvents.i.UpdateSquad();
        yield return new WaitForSeconds(15);   
    }

    [UnityTest]
    public IEnumerator PartialBarracksTest()
    {
        campaignData.playerUnits.Add(CreateTestDummy());
        campaignData.playerUnits.Add(CreateTestDummy());
        campaignData.playerUnits.Add(CreateTestDummy());
        CampaignEvents.i.UpdateSquad();
        yield return new WaitForSeconds(15);
    }

    [UnityTest]
    public IEnumerator UnequippedAbilitiesTest()
    {
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        CampaignEvents.i.UpdateAbilities();
        yield return new WaitForSeconds(5);
        campaignData.unequippedAbilities.RemoveAt(0);
        campaignData.unequippedAbilities.RemoveAt(0);
        campaignData.unequippedAbilities.RemoveAt(0);
        CampaignEvents.i.UpdateAbilities();
        yield return new WaitForSeconds(5);
        campaignData.unequippedAbilities.Add(CreateNewAbility(AbilityType.MINE));
        campaignData.unequippedAbilities.Add(CreateNewAbility(AbilityType.MINE));
        CampaignEvents.i.UpdateAbilities();
        yield return new WaitForSeconds(10);
    }

    [UnityTest]
    public IEnumerator AbilitiesTests()
    {
        campaignData.playerUnits.Add(CreateTestDummy());
        campaignData.playerUnits.Add(CreateTestDummy());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        campaignData.unequippedAbilities.Add(CreateNewAbility());
        CampaignEvents.i.UpdateAbilities();
        yield return new WaitForSeconds(20);
    }
}

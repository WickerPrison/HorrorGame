using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CampaignMenuTests
{
    CampaignData campaignData;
    TestingData testData;


    [SetUp]
    public void Setup()
    {
        testData = Resources.Load<TestingData>("Data/TestingData");
        campaignData = Resources.Load<CampaignData>("Data/CampaignData");
        Time.timeScale = testData.timeScale;
        campaignData.missions = null;
        SceneManager.LoadScene("MissionSelect");
    }

    PlayerUnitData CreateTestDummy()
    {
        return new PlayerUnitData("Test Dummy", 100, 0);
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
}

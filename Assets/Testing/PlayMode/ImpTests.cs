using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class ImpTests
{
    TestingData testData;
    GameObject enemyPrefab;
    ImpAi imp;
    GameObject playerUnitPrefab;
    PlayerUnit playerUnit;
    PlayerUnitData testDummyData;

    [SetUp]
    public void Setup()
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Imp");
        playerUnitPrefab = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        testData = Resources.Load<TestingData>("Data/TestingData");
        testDummyData = new PlayerUnitData("Test Dummy", 100, 0);
        //Time.timeScale = testData.timeScale;
    }

    public IEnumerator LoadBasicScene()
    {
        SceneManager.LoadScene("BasicTest");
        yield return null;
        imp = GameObject.Instantiate(enemyPrefab).GetComponent<ImpAi>();
        imp.transform.position = new Vector3(0, 0);
        TestingUtils.GiveRoomsVisionNodes();
        yield return null;
    }

    public void SpawnPlayerUnit(Vector2 spawnPosition)
    {
        playerUnit = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        playerUnit.data = testDummyData;
        playerUnit.transform.position = spawnPosition;
    }

    [UnityTest]
    public IEnumerator Attack()
    {
        yield return LoadBasicScene();
        SpawnPlayerUnit(new Vector2(-2.2f, -2.2f));
        yield return new WaitForSeconds(2f);
        Assert.Less(testDummyData.health, testDummyData.maxHealth);
    }
}

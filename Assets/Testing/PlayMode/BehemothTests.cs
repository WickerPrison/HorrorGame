using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class BehemothTests
{
    TestingData testData;
    GameObject enemyPrefab;
    Behemoth behemoth;
    GameObject playerUnitPrefab;
    PlayerUnit playerUnit;
    PlayerUnitData testDummyData;

    [SetUp]
    public void Setup()
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Behemoth");
        playerUnitPrefab = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        testData = Resources.Load<TestingData>("Data/TestingData");
        testDummyData = new PlayerUnitData("Test Dummy", 100, 0);
        //Time.timeScale = testData.timeScale;
    }

    public IEnumerator LoadTwoRoomScene()
    {
        SceneManager.LoadScene("TwoRooms");
        yield return null;
        behemoth = GameObject.Instantiate(enemyPrefab).GetComponent<Behemoth>();
        behemoth.transform.position = TestingUtils.GetLeftMostRoom().transform.position;
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
        yield return LoadTwoRoomScene();
        SpawnPlayerUnit(new Vector2(-3.2f, -3.2f));
        yield return new WaitForSeconds(3f);
        Assert.Less(testDummyData.health, testDummyData.maxHealth);
    }
}

using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MineTests
{
    TestingData testData;
    GameObject minePrefab;
    GameObject enemyPrefab;
    GameObject playerUnitPrefab;
    GameObject resourcePrefab;
    PlayerUnit playerUnit;
    PlayerUnitData testDummyData;
    Room room;

    [SetUp]
    public void Setup()
    {
        minePrefab = Resources.Load<GameObject>("Prefabs/Mine");
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        playerUnitPrefab = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        testData = Resources.Load<TestingData>("Data/TestingData");
        testDummyData = new PlayerUnitData("Test Dummy", 100, 0);
        resourcePrefab = Resources.Load<GameObject>("Prefabs/Resource");
        Time.timeScale = testData.timeScale;
    }

    public IEnumerator LoadBasicScene()
    {
        SceneManager.LoadScene("BasicTest");
        yield return null;
        room = Utils.GetRoom(Vector3.zero);
        TestingUtils.GiveRoomsVisionNodes();
        yield return null;
    }

    public IEnumerator LoadTwoRoomScene()
    {
        SceneManager.LoadScene("TwoRooms");
        yield return null;
        TestingUtils.GiveRoomsVisionNodes();
        yield return null;
    }

    [UnityTest]
    public IEnumerator MineDamagesEnemy()
    {
        yield return LoadBasicScene();
        Mine mine = GameObject.Instantiate(minePrefab).GetComponent<Mine>();
        mine.transform.position = new Vector3(3f, 3f);
        Enemy enemy = GameObject.Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.transform.position = new Vector3(-3f, -3f);
        enemy.SetTestingState();
        
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(enemy.health, enemy.maxHealth);

        enemy.GoTo(mine.transform.position);
        yield return new WaitForSeconds(3);

        Assert.Less(enemy.health, enemy.maxHealth);
    }

    [UnityTest]
    public IEnumerator MineDamagesPlayer()
    {
        yield return LoadBasicScene();
        Mine mine = GameObject.Instantiate(minePrefab).GetComponent<Mine>();
        mine.transform.position = new Vector3(3f, 3f);
        Enemy enemy = GameObject.Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.transform.position = new Vector3(-3f, -3f);
        enemy.SetTestingState();

        playerUnit = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        playerUnit.data = testDummyData;
        playerUnit.transform.position = new Vector3(-5f, 5f);

        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(playerUnit.data.health, playerUnit.data.maxHealth);

        enemy.GoTo(mine.transform.position);
        yield return new WaitForSeconds(3);

        Assert.Less(playerUnit.data.health, playerUnit.data.maxHealth);
    }

    [UnityTest]
    public IEnumerator MineDestroysResources()
    {
        yield return LoadBasicScene();
        Mine mine = GameObject.Instantiate(minePrefab).GetComponent<Mine>();
        mine.transform.position = new Vector3(3f, 3f);
        Enemy enemy = GameObject.Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.transform.position = new Vector3(-3f, -3f);
        enemy.SetTestingState();
        GameObject.Instantiate(resourcePrefab).transform.position = new Vector3(1, 5);
        GameObject.Instantiate(resourcePrefab).transform.position = new Vector3(3, 2);
        GameObject.Instantiate(resourcePrefab).transform.position = new Vector3(-4, -3);

        yield return new WaitForSeconds(0.5f);
        Assert.Greater(room.resources.Count, 0);

        enemy.GoTo(mine.transform.position);
        yield return new WaitForSeconds(3);

        Assert.AreEqual(room.resources.Count, 0);
    }

    [UnityTest]
    public IEnumerator DoesntTriggerThroughWalls()
    {
        yield return LoadTwoRoomScene();
        Mine mine = GameObject.Instantiate(minePrefab).GetComponent<Mine>();
        mine.transform.position = new Vector3(0.5f, 3f);
        Enemy enemy = GameObject.Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.transform.position = new Vector3(-1f, -3f);
        enemy.SetTestingState();

        yield return new WaitForSeconds(0.5f);

        enemy.GoTo(mine.transform.position);
        yield return new WaitForSeconds(3);
    }
}

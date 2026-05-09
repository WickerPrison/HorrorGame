using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class MineTest
{
    TestingData testData;
    GameObject minePrefab;
    GameObject enemyPrefab;
    GameObject playerUnitPrefab;
    GameObject resourcePrefab;
    PlayerUnit playerUnit;
    PlayerUnitData testDummyData;
    Room room;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("BasicTest");
        minePrefab = Resources.Load<GameObject>("Prefabs/Mine");
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        playerUnitPrefab = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        testData = Resources.Load<TestingData>("Data/TestingData");
        testDummyData = new PlayerUnitData("Test Dummy", 100);
        resourcePrefab = Resources.Load<GameObject>("Prefabs/Resource");
        Time.timeScale = testData.timeScale;

        yield return null;
        room = Utils.GetRoom(Vector3.zero);
        playerUnit = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        playerUnit.data = testDummyData;
        playerUnit.transform.position = new Vector3(-5f, 5f);
        playerUnit.visionRange = 100f;
        yield return null;
    }

    [UnityTest]
    public IEnumerator MineDamagesEnemy()
    {
        Mine mine = GameObject.Instantiate(minePrefab).GetComponent<Mine>();
        mine.transform.position = new Vector3(3f, 3f);
        Enemy enemy = GameObject.Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.transform.position = new Vector3(-3f, -3f);
        enemy.SetTestingState();
        
        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(enemy.health, enemy.maxHealth);

        enemy.GoTo(mine.transform.position);
        yield return new WaitForSeconds(3);

        Debug.Log(enemy.health);
        Assert.Less(enemy.health, enemy.maxHealth);
    }

    [UnityTest]
    public IEnumerator MineDamagesPlayer()
    {
        Mine mine = GameObject.Instantiate(minePrefab).GetComponent<Mine>();
        mine.transform.position = new Vector3(3f, 3f);
        Enemy enemy = GameObject.Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.transform.position = new Vector3(-3f, -3f);
        enemy.SetTestingState();

        yield return new WaitForSeconds(0.5f);
        Assert.AreEqual(playerUnit.data.health, playerUnit.data.maxHealth);

        enemy.GoTo(mine.transform.position);
        yield return new WaitForSeconds(3);

        Assert.Less(playerUnit.data.health, playerUnit.data.maxHealth);
    }

    [UnityTest]
    public IEnumerator MineDestroysResources()
    {
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
}

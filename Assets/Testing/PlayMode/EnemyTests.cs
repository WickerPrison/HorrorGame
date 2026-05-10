using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class EnemyTests
{
    TestingData testData;
    GameObject enemyPrefab;
    Enemy enemy;
    GameObject playerUnitPrefab;
    PlayerUnit playerUnit;
    PlayerUnitData testDummyData;

    [SetUp]
    public void Setup()
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        playerUnitPrefab = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        testData = Resources.Load<TestingData>("Data/TestingData");
        testDummyData = new PlayerUnitData("Test Dummy", 100);
        Time.timeScale = testData.timeScale;
    }

    public IEnumerator LoadBasicScene()
    {
        SceneManager.LoadScene("TwoRooms");
        yield return null;
        enemy = GameObject.Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.transform.position = new Vector3(-4f, 0);
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
    public IEnumerator EnemyAggro()
    {
        yield return LoadBasicScene();
        enemy.visionRange = 1f;
        SpawnPlayerUnit(new Vector2(-1.2f, -1.2f));
        yield return new WaitForSeconds(2);
        Assert.AreNotEqual(EnemyState.CHASING, enemy.state);

        enemy.visionRange = 10f;
        yield return new WaitForSeconds(0.2f);
        Assert.AreEqual(EnemyState.CHASING, enemy.state);

        playerUnit.transform.position = new Vector2(1, 3);
        enemy.GoTo(new Vector2(-1, 3));
        yield return new WaitForSeconds(2);
        Assert.AreNotEqual(EnemyState.CHASING, enemy.state);
    }
}

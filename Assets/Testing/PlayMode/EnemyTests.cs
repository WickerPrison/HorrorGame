using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EnemyTests
{
    TestingData testData;
    GameObject enemyPrefab;
    GameObject playerUnitPrefab;
    PlayerUnit playerUnit;
    PlayerUnitData testDummyData;
    Room room;

    [SetUp]
    public void Setup()
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        playerUnitPrefab = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        testData = Resources.Load<TestingData>("Data/TestingData");
        testDummyData = new PlayerUnitData("Test Dummy", 100);
        Time.timeScale = testData.timeScale;
    }
}

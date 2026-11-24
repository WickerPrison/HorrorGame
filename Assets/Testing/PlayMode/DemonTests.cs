using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class DemonTests
{
    TestingData testData;
    GameObject enemyPrefab;
    DemonAi demon;
    Enemy demonEnemy;
    GameObject impPrefab;
    GameObject devilSkullPrefab;
    GameObject behemothPrefab;
    GameObject playerUnitPrefab;
    PlayerUnit playerUnit;
    PlayerUnitData testDummyData;

    [SetUp]
    public void Setup()
    {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Demon");
        impPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Imp");
        devilSkullPrefab = Resources.Load<GameObject>("Prefabs/Enemies/DevilSkull");
        behemothPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Behemoth");
        playerUnitPrefab = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        testData = Resources.Load<TestingData>("Data/TestingData");
        testDummyData = new PlayerUnitData("Test Dummy", 400, 0);
        //Time.timeScale = testData.timeScale;
    }

    public IEnumerator LoadTwoRoomScene()
    {
        SceneManager.LoadScene("TwoRooms");
        yield return null;
        demon = GameObject.Instantiate(enemyPrefab).GetComponent<DemonAi>();
        demon.transform.position = TestingUtils.GetLeftMostRoom().transform.position;
        demonEnemy = demon.GetComponent<Enemy>();
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
        yield return new WaitForSeconds(3f);
    }

    [UnityTest]
    public IEnumerator FightWithImp()
    {
        yield return LoadTwoRoomScene();
        ImpAi imp = GameObject.Instantiate(impPrefab).GetComponent<ImpAi>();
        imp.transform.position = TestingUtils.GetLeftMostRoom().transform.position + new Vector3(3, 2, 0);
        yield return new WaitForSeconds(2);
        Enemy impEnemy = imp.GetComponent<Enemy>();
        Assert.Less(demonEnemy.health, demonEnemy.maxHealth);
        Assert.Less(impEnemy.health, impEnemy.maxHealth);
    }

    [UnityTest]
    public IEnumerator FightWithDevilSkull()
    {
        yield return LoadTwoRoomScene();
        DevilSkullAi devilSkull = GameObject.Instantiate(devilSkullPrefab).GetComponent<DevilSkullAi>();
        devilSkull.transform.position = TestingUtils.GetLeftMostRoom().transform.position + new Vector3(3, 2, 0);
        yield return null;
        Enemy devilSkullEnemy = devilSkull.GetComponent<Enemy>();
        Assert.IsTrue(MissionManager.i.enemies.Contains(demonEnemy));
        Assert.IsTrue(MissionManager.i.enemies.Contains(devilSkullEnemy));
        yield return new WaitForSeconds(2);
        Assert.IsFalse(MissionManager.i.enemies.Contains(demonEnemy));
        Assert.IsFalse(MissionManager.i.enemies.Contains(devilSkullEnemy));
    }

    [UnityTest]
    public IEnumerator FightWithBehemoth()
    {
        yield return LoadTwoRoomScene();
        Behemoth behemoth = GameObject.Instantiate(behemothPrefab).GetComponent<Behemoth>();
        behemoth.transform.position = TestingUtils.GetLeftMostRoom().transform.position + new Vector3(3, 2, 0);
        yield return new WaitForSeconds(2);
        Enemy behemothEnemy = behemoth.GetComponent<Enemy>();
        Assert.Less(demonEnemy.health, demonEnemy.maxHealth);
        Assert.Less(behemothEnemy.health, behemothEnemy.maxHealth);
    }

    [UnityTest]
    public IEnumerator FightWithDemon()
    {
        yield return LoadTwoRoomScene();
        DemonAi demon2 = GameObject.Instantiate(enemyPrefab).GetComponent<DemonAi>();
        demon2.transform.position = TestingUtils.GetLeftMostRoom().transform.position + new Vector3(3, 2, 0);
        yield return new WaitForSeconds(2);
        Enemy demon2Enemy = demon2.GetComponent<Enemy>();
        Assert.Less(demonEnemy.health, demonEnemy.maxHealth);
        Assert.Less(demon2Enemy.health, demon2Enemy.maxHealth);
    }

    [UnityTest]
    public IEnumerator FightWithImpAndPlayer()
    {
        yield return LoadTwoRoomScene();
        ImpAi imp = GameObject.Instantiate(impPrefab).GetComponent<ImpAi>();
        Vector2 roomPos = TestingUtils.GetLeftMostRoom().transform.position;
        imp.transform.position = roomPos + new Vector2(3, 2);
        SpawnPlayerUnit(roomPos + new Vector2(-2, 2));
        yield return new WaitForSeconds(2);
        Enemy impEnemy = imp.GetComponent<Enemy>();
        Assert.Less(demonEnemy.health, demonEnemy.maxHealth);
        Assert.Less(testDummyData.health, testDummyData.maxHealth);
    }
}

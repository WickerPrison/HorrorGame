using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class HolyAuraTests
{
    TestingData testData;
    Altar altar;
    Room originRoom;
    Room room1;
    Room room2;
    Room room3;
    Door door1;
    Door door2;
    Door door3;
    GameObject playerUnitPrefab;
    GameObject enemyPrefab;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        playerUnitPrefab = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        testData = Resources.Load<TestingData>("Data/TestingData");
        Time.timeScale = testData.timeScale;
        SceneManager.LoadScene("HellfireTests");
        yield return null;
        altar = GameObject.FindAnyObjectByType<Altar>();
        altar.Sanctify();
        originRoom = altar.room;
        door1 = originRoom.doors[0];
        room1 = door1.roomDict[originRoom];
        door2 = room1.doors.Where(d => d != door1).ToArray()[0];
        room2 = door2.roomDict[room1];
        door3 = room2.doors.Where(d => d != door2).ToArray()[0];
        room3 = door3.roomDict[room2];
    }


    [UnityTest]
    public IEnumerator HolyAuraBuildsInAltarRoom()
    {
        Assert.Less(originRoom.dot, 2);
        Assert.Less(room1.dot, 1);
        yield return new WaitForSeconds(15);
        Assert.GreaterOrEqual(originRoom.dot, 3);
        Assert.Less(room1.dot, 1);
    }

    [UnityTest]
    public IEnumerator HolyAuraSpreadsThroughOpenDoor()
    {
        Assert.Less(room1.dot, 1);
        originRoom.SetDot(3.5f);
        door1.OpenDoor();
        door2.OpenDoor();
        door3.OpenDoor();
        yield return new WaitForSeconds(35);
        Assert.AreEqual(Mathf.FloorToInt(room1.dot), 2);
        Assert.AreEqual(Mathf.FloorToInt(room2.dot), 1);
        Assert.AreEqual(Mathf.FloorToInt(room3.dot), 0);
        door1.CloseDooor();
        door2.CloseDooor();
        door3.CloseDooor();
        yield return new WaitForSeconds(10);
        Assert.Less(room1.dot, 1);
        Assert.Less(room2.dot, 1);
        Assert.Less(room3.dot, 1);
    }

    [UnityTest]
    public IEnumerator TakeHolyAuraDamage()
    {
        int maxHealth = 100;

        PlayerUnit neutral = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        neutral.data = new PlayerUnitData("Neutral", maxHealth, 0);
        neutral.transform.position = room1.transform.position;

        PlayerUnit good = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        good.data = new PlayerUnitData("Good", maxHealth, 1, 5);
        good.transform.position = room1.transform.position + new Vector3(1f, 1f);

        PlayerUnit evil = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        evil.data = new PlayerUnitData("Evil", maxHealth, 2, -5);
        evil.transform.position = room1.transform.position + new Vector3(-1f, 0);

        PlayerUnit superGood = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        superGood.data = new PlayerUnitData("Super Good", maxHealth, 3, 10);
        superGood.transform.position = room1.transform.position + new Vector3(-1f, -1f);
        yield return null;

        neutral.TakeHolyAuraDamage(3);
        Assert.Less(neutral.data.health, maxHealth);

        evil.TakeHolyAuraDamage(3);
        Assert.Less(evil.data.health, neutral.data.health);

        good.TakeHolyAuraDamage(3);
        Assert.Less(good.data.health, maxHealth);
        Assert.Greater(good.data.health, neutral.data.health);

        superGood.TakeHolyAuraDamage(3);
        Assert.AreEqual(maxHealth, superGood.data.health);
        yield return new WaitForSeconds(5);
    }

    [UnityTest]
    public IEnumerator RoomDealsHolyAuraDamage()
    {
        int maxHealth = 100;

        PlayerUnit neutral = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        neutral.data = new PlayerUnitData("Neutral", maxHealth, 0);
        neutral.transform.position = originRoom.transform.position;

        PlayerUnit good = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        good.data = new PlayerUnitData("Good", maxHealth, 1, 5);
        good.transform.position = originRoom.transform.position + new Vector3(1f, 1f);

        PlayerUnit evil = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        evil.data = new PlayerUnitData("Evil", maxHealth, 2, -5);
        evil.transform.position = originRoom.transform.position + new Vector3(-1f, 0);

        PlayerUnit superGood = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        superGood.data = new PlayerUnitData("Super Good", maxHealth, 3, 10);
        superGood.transform.position = originRoom.transform.position + new Vector3(-1f, -1f);
        yield return new WaitForSeconds(10f);

        Assert.Less(neutral.data.health, maxHealth);
        Assert.Less(evil.data.health, neutral.data.health);
        Assert.Less(good.data.health, maxHealth);
        Assert.Greater(good.data.health, neutral.data.health);
        Assert.AreEqual(maxHealth, superGood.data.health);
    }

    [UnityTest]
    public IEnumerator HolyAuraKillsDemons()
    {
        Enemy enemy = GameObject.Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.transform.position = originRoom.transform.position + new Vector3(-2f, 0);
        yield return new WaitForSeconds(5);
        Assert.Less(enemy.health, enemy.maxHealth);
    }
}

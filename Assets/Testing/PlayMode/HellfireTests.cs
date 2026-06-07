using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;
using UnityEngine.SceneManagement;

public class HellfireTests
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

    [UnitySetUp]
    public IEnumerator Setup()
    {
        playerUnitPrefab = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        testData = Resources.Load<TestingData>("Data/TestingData");
        Time.timeScale = testData.timeScale;
        SceneManager.LoadScene("HellfireTests");
        yield return null;
        altar = GameObject.FindAnyObjectByType<Altar>();
        altar.Desecrate();
        originRoom = altar.room;
        door1 = originRoom.doors[0];
        room1 = door1.roomDict[originRoom];
        door2 = room1.doors.Where(d => d != door1).ToArray()[0];
        room2 = door2.roomDict[room1];
        door3 = room2.doors.Where(d => d != door2).ToArray()[0];
        room3 = door3.roomDict[room2];
    }


    [UnityTest]
    public IEnumerator HellfireBuildsInAltarRoom()
    {
        Assert.Greater(originRoom.dot, -2);
        Assert.Greater(room1.dot, -1);
        yield return new WaitForSeconds(15);
        Assert.LessOrEqual(originRoom.dot, -3);
        Assert.Greater(room1.dot, -1);
    }

    [UnityTest]
    public IEnumerator HellfireSpreadsThroughOpenDoor()
    {
        Assert.Greater(room1.dot, -1);
        originRoom.SetDot(-3.5f);
        door1.OpenDoor();
        door2.OpenDoor();
        door3.OpenDoor();
        yield return new WaitForSeconds(35);
        Assert.AreEqual(Mathf.CeilToInt(room1.dot), -2);
        Assert.AreEqual(Mathf.CeilToInt(room2.dot), -1);
        Assert.AreEqual(Mathf.CeilToInt(room3.dot), 0);
        door1.CloseDooor();
        door2.CloseDooor();
        door3.CloseDooor();
        yield return new WaitForSeconds(10);
        Assert.Greater(room1.dot, -1);
        Assert.Greater(room2.dot, -1);
        Assert.Greater(room3.dot, -1);
    }

    [UnityTest]
    public IEnumerator TakeHellfireDamage()
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

        PlayerUnit superEvil = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        superEvil.data = new PlayerUnitData("Super Evil", maxHealth, 3, -10);
        superEvil.transform.position = room1.transform.position + new Vector3(-1f, -1f);
        yield return null;

        neutral.TakeHellfireDamage(-3);
        Assert.Less(neutral.data.health, maxHealth);

        good.TakeHellfireDamage(-3);
        Assert.Less(good.data.health, neutral.data.health);

        evil.TakeHellfireDamage(-3);
        Assert.Less(evil.data.health, maxHealth);
        Assert.Greater(evil.data.health, neutral.data.health);

        superEvil.TakeHellfireDamage(-3);
        Assert.AreEqual(maxHealth, superEvil.data.health);
        yield return new WaitForSeconds(5);
    }

    [UnityTest]
    public IEnumerator RoomDealsHellfireDamage()
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

        PlayerUnit superEvil = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        superEvil.data = new PlayerUnitData("Super Evil", maxHealth, 3, -10);
        superEvil.transform.position = originRoom.transform.position + new Vector3(-1f, -1f);
        yield return new WaitForSeconds(10f);

        Assert.Less(neutral.data.health, maxHealth);
        Assert.Less(good.data.health, neutral.data.health);
        Assert.Less(evil.data.health, maxHealth);
        Assert.Greater(evil.data.health, neutral.data.health);
        Assert.AreEqual(maxHealth, superEvil.data.health);
    }
}

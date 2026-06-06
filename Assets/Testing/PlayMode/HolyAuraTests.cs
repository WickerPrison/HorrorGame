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
    Room otherRoom;
    Door door;
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
        altar.Sanctify();
        originRoom = altar.room;
        otherRoom = GameObject.FindObjectsByType<Room>(FindObjectsSortMode.None).Where(r => r != originRoom).ToArray()[0];
        door = originRoom.doors[0];
    }


    [UnityTest]
    public IEnumerator HolyAuraBuildsInAltarRoom()
    {
        Assert.Less(originRoom.dot, 2);
        Assert.Less(otherRoom.dot, 1);
        yield return new WaitForSeconds(15);
        Assert.GreaterOrEqual(originRoom.dot, 3);
        Assert.Less(otherRoom.dot, 1);
    }

    [UnityTest]
    public IEnumerator HolyAuraSpreadsThroughOpenDoor()
    {
        Assert.Less(otherRoom.dot, 1);
        originRoom.dot = 3;
        originRoom.GainHolyAura(4);
        door.OpenDoor();
        yield return new WaitForSeconds(19);
        Assert.Greater(otherRoom.dot, 2);
        door.CloseDooor();
        yield return new WaitForSeconds(15);
        Assert.Less(otherRoom.dot, 1);
    }

    [UnityTest]
    public IEnumerator TakeHolyAuraDamage()
    {
        int maxHealth = 100;

        PlayerUnit neutral = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        neutral.data = new PlayerUnitData("Neutral", maxHealth, 0);
        neutral.transform.position = otherRoom.transform.position;

        PlayerUnit good = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        good.data = new PlayerUnitData("Good", maxHealth, 1, 5);
        good.transform.position = otherRoom.transform.position + new Vector3(1f, 1f);

        PlayerUnit evil = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        evil.data = new PlayerUnitData("Evil", maxHealth, 2, -5);
        evil.transform.position = otherRoom.transform.position + new Vector3(-1f, 0);

        PlayerUnit superGood = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        superGood.data = new PlayerUnitData("Super Good", maxHealth, 3, 10);
        superGood.transform.position = otherRoom.transform.position + new Vector3(-1f, -1f);
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
}

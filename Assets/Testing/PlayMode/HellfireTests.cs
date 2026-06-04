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
    Room otherRoom;
    Door door;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        testData = Resources.Load<TestingData>("Data/TestingData");
        Time.timeScale = testData.timeScale;
        SceneManager.LoadScene("HellfireTests");
        yield return null;
        altar = GameObject.FindAnyObjectByType<Altar>();
        originRoom = altar.room;
        otherRoom = GameObject.FindObjectsByType<Room>(FindObjectsSortMode.None).Where(r => r != originRoom).ToArray()[0];
        door = originRoom.doors[0];
    }


    [UnityTest]
    public IEnumerator HellfireBuildsInAltarRoom()
    {
        Assert.Less(originRoom.hellfire, 1);
        Assert.Less(otherRoom.hellfire, 1);
        yield return new WaitForSeconds(19);
        Assert.GreaterOrEqual(originRoom.hellfire, 3);
        Assert.Less(otherRoom.hellfire, 1);
    }

    [UnityTest]
    public IEnumerator HellfireSpreadsThroughOpenDoor()
    {
        Assert.Less(otherRoom.hellfire, 1);
        originRoom.hellfire = 3;
        originRoom.GainHellfire(4);
        door.OpenDoor();
        yield return new WaitForSeconds(19);
        Assert.Greater(otherRoom.hellfire, 2);
        door.CloseDooor();
        yield return new WaitForSeconds(15);
        Assert.Less(otherRoom.hellfire, 1);
    }
}

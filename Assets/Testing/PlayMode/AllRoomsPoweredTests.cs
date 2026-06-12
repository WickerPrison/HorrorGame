using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class AllRoomsPoweredTests
{
    IEnumerator AllRoomsHavePower(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        yield return null;
        Terminal[] terminals = Object.FindObjectsByType<Terminal>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        Room[] rooms = Object.FindObjectsByType<Room>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        yield return null;
        bool allRoomsHavePower = true;
        foreach(Room room in rooms)
        {
            bool roomHasPower = false;
            if(room.portal != null && room.portal.providesPower)
            {
                roomHasPower = true;
                continue;
            }

            bool poweredByTerminal = false;
            foreach(Terminal terminal in terminals)
            {
                if (terminal.roomsToPower.Contains(room))
                {
                    poweredByTerminal = true;
                    break;
                }
            }
            if (poweredByTerminal)
            {
                roomHasPower = true;
                continue;
            }

            if (!roomHasPower)
            {
                Debug.Log($"Room {room.name} does not have power");
                allRoomsHavePower = false;
            }
        }
        Assert.IsTrue(allRoomsHavePower);
    }

    [UnityTest]
    public IEnumerator Level1()
    {
        yield return AllRoomsHavePower("Level1");
    }

    [UnityTest]
    public IEnumerator Level2()
    {
        yield return AllRoomsHavePower("Level2");
    }
}

using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class AltarTests
{
    TestingData testData;
    Altar altar;
    Room originRoom;
    Room otherRoom;
    Door door;
    GameObject playerUnitPrefab;
    AbilityDictionary abilityDictionary;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        playerUnitPrefab = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        testData = Resources.Load<TestingData>("Data/TestingData");
        abilityDictionary = Resources.Load<AbilityDictionary>("Data/Abilities/AbilityDictionary");
        Time.timeScale = testData.timeScale;
        SceneManager.LoadScene("HellfireTests");
        yield return null;
        altar = GameObject.FindAnyObjectByType<Altar>();
        originRoom = altar.room;
        otherRoom = GameObject.FindObjectsByType<Room>(FindObjectsSortMode.None).Where(r => r != originRoom).ToArray()[0];
        door = originRoom.doors[0];
    }

    Ability[] GetAbilityArray(AbilityType[] types)
    {
        return types.Select(abilityType => new Ability(abilityDictionary, abilityType)).ToArray();
    }


    [UnityTest]
    public IEnumerator SanctifyAltar()
    {
        int maxHealth = 100;
        altar.Desecrate();
        PlayerUnit good = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        good.data = new PlayerUnitData("Good", maxHealth, 1, 5, GetAbilityArray(new AbilityType[] { AbilityType.SANCTIFY, AbilityType.NONE, AbilityType.NONE, AbilityType.NONE }));
        good.transform.position = originRoom.transform.position + new Vector3(2f, 2f);
        yield return new WaitForSeconds(1);

        Assert.Less(originRoom.dot, -1);
        good.PerformAbility(0);
        
        yield return new WaitForSeconds(7);
        Assert.Greater(originRoom.dot, 1);
    }

    [UnityTest]
    public IEnumerator DesecrateAltar()
    {
        int maxHealth = 100;
        altar.Sanctify();
        PlayerUnit evil = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        evil.data = new PlayerUnitData("Evil", maxHealth, 1, -5, GetAbilityArray(new AbilityType[] { AbilityType.DESECRATE, AbilityType.NONE, AbilityType.NONE, AbilityType.NONE }));
        evil.transform.position = originRoom.transform.position + new Vector3(-3f, 2f);
        yield return new WaitForSeconds(1);

        Assert.Greater(originRoom.dot, 1);
        evil.PerformAbility(0);

        yield return new WaitForSeconds(7);
        Assert.Less(originRoom.dot, -1);
    }

    [UnityTest]
    public IEnumerator CanUseRightClick()
    {
        int maxHealth = 100;
        altar.Desecrate();
        PlayerUnit good = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        good.data = new PlayerUnitData("Good", maxHealth, 0, 5, GetAbilityArray(new AbilityType[] { AbilityType.SANCTIFY, AbilityType.NONE, AbilityType.NONE, AbilityType.NONE }));
        good.transform.position = originRoom.transform.position + new Vector3(2f, 2f);
        yield return new WaitForSeconds(1);

        Assert.Less(originRoom.dot, -1);
        PlayerManager.i.SelectButton(0);
        InputManager.i.RightClick(altar.transform.position);

        yield return new WaitForSeconds(7);
        Assert.Greater(originRoom.dot, 1);
    }
}

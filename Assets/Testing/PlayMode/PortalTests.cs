using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PortalTests
{
    TestingData testData;
    GameObject enemyPrefab;
    GameObject playerUnitPrefab;
    List<Room> portalRooms;
    PlayerManager playerManager;

    [UnitySetUp]
    public IEnumerator Setup()
    {
        SceneManager.LoadScene("PortalTest");
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        playerUnitPrefab = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        testData = Resources.Load<TestingData>("Data/TestingData");
        Time.timeScale = testData.timeScale;

        yield return null;
        playerManager = PlayerEvents.i.GetComponent<PlayerManager>();
        portalRooms = Utils.GetRooms(Vector2.zero, 100).Where(r => r.portal != null).ToList();
    }

    PlayerUnit SpawnUnit(Vector2 position, int index, string name = "Test Dummy")
    {
        PlayerUnit playerUnit = GameObject.Instantiate(playerUnitPrefab).GetComponent<PlayerUnit>();
        PlayerUnitData testDummyData = new PlayerUnitData(name, 100, index);
        playerUnit.data = testDummyData;
        playerUnit.transform.position = position;
        return playerUnit;
    }

    [UnityTest]
    public IEnumerator UnitActivatesPortal()
    {
        PlayerUnit playerUnit = SpawnUnit((Vector2)portalRooms[0].portal.transform.position + new Vector2(1.3f, 1.3f), 0);
        yield return null;
        Assert.IsNull(PortalManager.i.activePortal);
        playerManager.SelectButton(0);
        playerManager.Portal();
        yield return new WaitForSeconds(2f);
        Assert.IsNotNull(PortalManager.i.activePortal);
        Assert.AreEqual(playerUnit, PortalManager.i.activator);
    }

    [UnityTest]
    public IEnumerator SecondUnitCannotActivateSamePortal()
    {
        PlayerUnit playerUnit1 = SpawnUnit((Vector2)portalRooms[0].portal.transform.position + new Vector2(1.3f, 1.3f), 0, "unit 1");
        PlayerUnit playerUnit2 = SpawnUnit((Vector2)portalRooms[0].portal.transform.position + new Vector2(-1.3f, -1.3f), 1, "unit 2");
        yield return null;
        Assert.IsNull(PortalManager.i.activePortal);
        playerManager.SelectButton(0);
        playerManager.Portal();
        yield return new WaitForSeconds(1f);
        Assert.IsNotNull(PortalManager.i.activePortal);
        Assert.AreEqual(playerUnit1, PortalManager.i.activator);
        playerManager.SelectButton(1);
        playerManager.Portal();
        yield return new WaitForSeconds(1f);
        Assert.IsNotNull(PortalManager.i.activePortal);
        Assert.AreEqual(playerUnit1, PortalManager.i.activator); 
    }

    [UnityTest]
    public IEnumerator SecondUnitCannotActivateDifferentPortal()
    {
        PlayerUnit playerUnit1 = SpawnUnit((Vector2)portalRooms[0].portal.transform.position + new Vector2(1.3f, 1.3f), 0, "unit 1");
        PlayerUnit playerUnit2 = SpawnUnit((Vector2)portalRooms[1].portal.transform.position + new Vector2(-1.3f, -1.3f), 1, "unit 2");
        yield return null;
        Assert.IsNull(PortalManager.i.activePortal);
        playerManager.SelectButton(0);
        playerManager.Portal();
        yield return new WaitForSeconds(1f);
        Assert.IsNotNull(PortalManager.i.activePortal);
        Assert.AreEqual(playerUnit1, PortalManager.i.activator);
        playerManager.SelectButton(1);
        playerManager.Portal();
        yield return new WaitForSeconds(1f);
        Assert.IsNotNull(PortalManager.i.activePortal);
        Assert.AreEqual(playerUnit1, PortalManager.i.activator);
    }

    [UnityTest]
    public IEnumerator UnitsOnPortalsSwitchPlaces()
    {
        PlayerUnit playerUnit1 = SpawnUnit(portalRooms[0].portal.transform.position, 0, "unit 1");
        PlayerUnit playerUnit2 = SpawnUnit(portalRooms[1].portal.transform.position, 1, "unit 2");
        yield return null;
        playerManager.SelectButton(0);
        playerManager.Portal();
        yield return new WaitForSeconds(1f);
        Assert.IsNotNull(PortalManager.i.activePortal);
        InputManager.i.RightClick(portalRooms[1].portal.transform.position);
        yield return new WaitForSeconds(1f);
        Assert.IsNull(PortalManager.i.activePortal);
        Assert.AreEqual(portalRooms[1].portal.transform.position, playerUnit1.transform.position);
        Assert.AreEqual(portalRooms[0].portal.transform.position, playerUnit2.transform.position);
    }

    [UnityTest]
    public IEnumerator CanTeleportEnemies()
    {
        PlayerUnit playerUnit = SpawnUnit(portalRooms[0].portal.transform.position, 0);
        Enemy enemy = GameObject.Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.transform.position = portalRooms[1].portal.transform.position;
        enemy.SetTestingState();
        yield return null;
        playerManager.SelectButton(0);
        playerManager.Portal();
        yield return new WaitForSeconds(1f);
        Assert.IsNotNull(PortalManager.i.activePortal);
        InputManager.i.RightClick(portalRooms[1].portal.transform.position);
        yield return new WaitForSeconds(1f);
        Assert.IsNull(PortalManager.i.activePortal);
        Assert.AreEqual(portalRooms[1].portal.transform.position, playerUnit.transform.position);
        Assert.AreEqual(portalRooms[0].portal.transform.position, enemy.transform.position);
    }

    [UnityTest]
    public IEnumerator UnitCanLeaveMission()
    {
        PlayerUnit playerUnit1 = SpawnUnit((Vector2)portalRooms[0].portal.transform.position + new Vector2(1.3f, 1.3f), 0, "unit 1");
        PlayerUnit playerUnit2 = SpawnUnit((Vector2)portalRooms[0].portal.transform.position + new Vector2(-1.3f, -1.3f), 1, "unit 2");
        yield return null;
        Assert.IsNull(PortalManager.i.activePortal);
        playerManager.SelectButton(0);
        playerManager.Portal();
        yield return new WaitForSeconds(1f);
        Assert.IsNotNull(PortalManager.i.activePortal);
        Assert.AreEqual(playerUnit1, PortalManager.i.activator);
        PortalManager.i.LeaveMission();
        yield return new WaitForSeconds(1f);
        Assert.IsNull(PortalManager.i.activePortal);
        Assert.IsNull(PortalManager.i.activator);
        Assert.AreEqual(1, playerManager.allUnits.Where(u => u != null).ToList().Count);
        Assert.AreEqual(0, playerManager.selectedUnits.Count);
    }

    [UnityTest]
    public IEnumerator CanLeaveMissionWithEnemies()
    {
        PlayerUnit playerUnit = SpawnUnit(portalRooms[0].portal.transform.position, 0);
        Enemy enemy = GameObject.Instantiate(enemyPrefab).GetComponent<Enemy>();
        enemy.transform.position = portalRooms[1].portal.transform.position;
        enemy.SetTestingState();
        yield return null;
        playerManager.SelectButton(0);
        playerManager.Portal();
        yield return new WaitForSeconds(1f);
        Assert.IsNotNull(PortalManager.i.activePortal);
        PortalManager.i.LeaveMission();
        yield return new WaitForSeconds(1f);
        Assert.IsTrue(enemy == null);
    }

    [UnityTest]
    public IEnumerator CanSelectUnitOnPortal()
    {
        PlayerUnit playerUnit = SpawnUnit(portalRooms[0].portal.transform.position, 0);

        yield return new WaitForFixedUpdate();
        InputManager.i.LeftClick(playerUnit.transform.position);
        yield return new WaitForSeconds(1f);
        Assert.Greater(PlayerManager.i.selectedUnits.Count, 0);
    }
}

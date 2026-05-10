using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class VisionManagerTests
{
    TestingData testData;
    GameObject minePrefab;
    GameObject enemyPrefab;
    GameObject playerUnitPrefab;
    GameObject resourcePrefab;
    GameObject visionNodePrefab;
    VisionNode visionNode;
    GameObject testTokenPrefab;
    PlayerUnit playerUnit;
    PlayerUnitData testDummyData;
    Room room;

    [SetUp]
    public void Setup()
    {
        minePrefab = Resources.Load<GameObject>("Prefabs/Mine");
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        playerUnitPrefab = Resources.Load<GameObject>("Prefabs/PlayerUnit");
        testData = Resources.Load<TestingData>("Data/TestingData");
        testDummyData = new PlayerUnitData("Test Dummy", 100);
        resourcePrefab = Resources.Load<GameObject>("Prefabs/Resource");
        visionNodePrefab = Resources.Load<GameObject>("Prefabs/VisionNode");
        testTokenPrefab = Resources.Load<GameObject>("Prefabs/TestToken");
        Time.timeScale = testData.timeScale;
    }

    public IEnumerator LoadTwoRoomScene()
    {
        SceneManager.LoadScene("TwoRooms");
        yield return null;
        room = Utils.GetRoom(Vector3.zero);
        visionNode = GameObject.Instantiate(visionNodePrefab).GetComponent<VisionNode>();
        yield return null;
        visionNode.visionRange = 5f;
    }

    public void SetTestTokenColor(SpriteRenderer testToken, bool isVisible)
    {
        if (isVisible)
        {
            testToken.color = Color.green;
        }
        else
        {
            testToken.color = Color.red;
        }
    }

    [UnityTest]
    public IEnumerator FindIsVisible()
    {
        yield return LoadTwoRoomScene();
        visionNode.transform.position = new Vector3(-1, 3);
        visionNode.ShowSprite(true);
        yield return null;

        // Sees thing in range
        Vector3 testPos = new Vector3(-2, 3);
        SpriteRenderer testToken1 = GameObject.Instantiate(testTokenPrefab).GetComponent<SpriteRenderer>();
        testToken1.transform.position = testPos;
        bool isVisible = VisionManager.i.FindIsVisible(testPos);
        SetTestTokenColor(testToken1, isVisible);
        Assert.IsTrue(isVisible);

        // Does not see through walls
        testPos = new Vector3(1, 3);
        SpriteRenderer testToken2 = GameObject.Instantiate(testTokenPrefab).GetComponent<SpriteRenderer>();
        testToken2.transform.position = testPos;
        isVisible = VisionManager.i.FindIsVisible(testPos);
        SetTestTokenColor(testToken2, isVisible);
        Assert.IsFalse(isVisible);

        //Does not see things out of range
        testPos = new Vector3(-4, -4);
        SpriteRenderer testToken3 = GameObject.Instantiate(testTokenPrefab).GetComponent<SpriteRenderer>();
        testToken3.transform.position = testPos;
        isVisible = VisionManager.i.FindIsVisible(testPos);
        SetTestTokenColor(testToken3, isVisible);
        Assert.Greater(Vector2.Distance(visionNode.transform.position, testPos), visionNode.visionRange);
        Assert.IsFalse(isVisible);
        yield return new WaitForSeconds(3f);
    }
}

using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class SlashAnimationTests
{
    TestingData testData;
    GameObject slashPrefab;
    SlashAnimation slash;

    bool testingBool = false;

    [SetUp]
    public void Setup()
    {
        slashPrefab = Resources.Load<GameObject>("Prefabs/Enemies/Slash");
        testData = Resources.Load<TestingData>("Data/TestingData");
        //Time.timeScale = testData.timeScale;
    }

    public IEnumerator LoadBasicScene()
    {
        SceneManager.LoadScene("BasicTest");
        yield return null;
        slash = GameObject.Instantiate(slashPrefab).GetComponent<SlashAnimation>();
        slash.transform.position = Vector3.zero;
        slash.stopAttackingMethod = () => { };
        TestingUtils.GiveRoomsVisionNodes();
        yield return null;
    }

    void SetTestingBool(bool hit)
    {
        testingBool = hit;
    }

    [UnityTest]
    public IEnumerator HitsTargetInFront()
    {
        yield return LoadBasicScene();
        testingBool = false;
        Transform target = new GameObject().transform;
        target.position = new Vector3(1, 0, 0);
        slash.Animation(Vector3.zero, target, SetTestingBool);
        yield return new WaitForSeconds(1f);
        Assert.IsTrue(testingBool);
    }

    [UnityTest]
    public IEnumerator CantHitOutOfRange()
    {
        yield return LoadBasicScene();
        testingBool = false;
        Transform target = new GameObject().transform;
        target.position = new Vector3(1.5f, 0, 0);
        slash.Animation(Vector3.zero, target, SetTestingBool);
        yield return new WaitForSeconds(1f);
        Assert.IsFalse(testingBool);
    }

    [UnityTest]
    public IEnumerator CantHitOutOfAngle()
    {
        yield return LoadBasicScene();
        testingBool = false;
        Transform target = new GameObject().transform;
        target.position = Vector3.right;

        Vector3 direction = Utils.RotateDirection(Vector3.right, 50);
        target.position = direction.normalized;

        slash.Animation(Vector3.zero, Vector3.right, target, SetTestingBool);
        yield return new WaitForSeconds(1f);
        Assert.IsFalse(testingBool);
    }

    [UnityTest]
    public IEnumerator HitsTargetInAngle()
    {
        yield return LoadBasicScene();
        testingBool = false;
        Transform target = new GameObject().transform;
        target.position = Vector3.right;

        Vector3 direction = Utils.RotateDirection(Vector3.right, 40);
        target.position = direction.normalized;

        slash.Animation(Vector3.zero, Vector3.right, target, SetTestingBool);
        yield return new WaitForSeconds(1f);
        Assert.IsTrue(testingBool);
    }
}

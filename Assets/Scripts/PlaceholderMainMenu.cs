using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceholderMainMenu : MonoBehaviour
{
    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial1");
    }

    public void TestLevel(int levelNum)
    {
        SceneManager.LoadScene($"TestLevel{levelNum}");
    }
}

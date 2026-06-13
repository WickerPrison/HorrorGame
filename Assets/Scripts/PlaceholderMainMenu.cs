using UnityEngine;
using UnityEngine.SceneManagement;

public class PlaceholderMainMenu : MonoBehaviour
{
    [SerializeField] LevelDetails levelDetails;
    [SerializeField] LevelsData levelsData;

    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial1");
    }

    public void TestLevel(int levelNum)
    {
        SceneManager.LoadScene($"TestLevel{levelNum}");
    }

    public void EasyRandomLevel()
    {
        levelDetails.threatLevel = ThreatLevel.LOW;
        levelDetails.interference = Interference.LOW;
        levelDetails.rewards = Rewards.MEDIUM;
        levelDetails.openness = Openness.LOW;

        LoadRandomScene();
    }

    public void MediumRandomLevel()
    {
        levelDetails.threatLevel = ThreatLevel.MEDIUM;
        levelDetails.interference = Interference.MEDIUM;
        levelDetails.rewards = Rewards.MEDIUM;
        levelDetails.openness = Openness.MEDIUM;

        LoadRandomScene();
    }

    public void HardRandomLevel()
    {
        levelDetails.threatLevel = ThreatLevel.HIGH;
        levelDetails.interference = Interference.HIGH;
        levelDetails.rewards = Rewards.MEDIUM;
        levelDetails.openness = Openness.HIGH;

        LoadRandomScene();
    }

    void LoadRandomScene()
    {
        int sceneId = Random.Range(1, levelsData.randomLevelCount + 1);

        SceneManager.LoadScene($"Level{sceneId}");
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Mission : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI missionName;
    [SerializeField] TextMeshProUGUI threatLevel;
    [SerializeField] TextMeshProUGUI resourceLevel;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] TextMeshProUGUI interference;
    [SerializeField] ColorData colorData;
    [SerializeField] LevelsData levelsData;
    [SerializeField] LevelDetails levelDetails;
    [SerializeField] CampaignData campaignData;

    [System.NonSerialized] public LevelDetailsData data;

    private void Start()
    {
        SetText();
    }

    void SetText()
    {
        missionName.text = data.missionName;
        threatLevel.text = $"Threat Level: {LevelDetailsData.GetString(data.threatLevel, colorData)}";
        resourceLevel.text = $"Resources: {LevelDetailsData.GetString(data.rewards)}";
        cost.text = $"Cost: {data.cost}";
        interference.text = $"Interference: {LevelDetailsData.GetString(data.interference, colorData)}";
    }

    public void StartMission()
    {
        if (campaignData.brimstone < data.cost) return;
        campaignData.brimstone -= data.cost;
        CampaignEvents.i.UpdateResources();
        levelDetails.SetData(data);
        int sceneId = Random.Range(1, levelsData.randomLevelCount + 1);
        campaignData.testingLevel = false;
        SceneManager.LoadScene($"Level{sceneId}");
    }
}

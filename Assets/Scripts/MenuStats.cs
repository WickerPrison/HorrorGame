using UnityEngine;
using TMPro;

public class MenuStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resources;
    [SerializeField] CampaignData campaignData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateStats();
    }

    void UpdateStats()
    {
        resources.text = $"Resources: {campaignData.resources}";
    }
}

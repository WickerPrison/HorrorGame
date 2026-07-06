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

    private void Campaign_onUpdateResources()
    {
        UpdateStats();
    }

    private void OnEnable()
    {
        CampaignEvents.i.onUpdateResources += Campaign_onUpdateResources;
    }

    private void OnDisable()
    {
        CampaignEvents.i.onUpdateResources -= Campaign_onUpdateResources;
    }
}

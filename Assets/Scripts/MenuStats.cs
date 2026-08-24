using UnityEngine;
using TMPro;

public class MenuStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI aether;
    [SerializeField] CampaignData campaignData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateStats();
    }

    void UpdateStats()
    {
        aether.text = $"Aether: {campaignData.aether}";
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

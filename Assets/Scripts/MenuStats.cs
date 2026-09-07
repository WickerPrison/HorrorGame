using UnityEngine;
using TMPro;

public class MenuStats : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI aether;
    [SerializeField] TextMeshProUGUI brimstone;
    [SerializeField] TextMeshProUGUI quintessence;
    [SerializeField] CampaignData campaignData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateStats();
    }

    void UpdateStats()
    {
        aether.text = $": {campaignData.aether}";
        brimstone.text = $": {campaignData.brimstone}";
        quintessence.text = $": {campaignData.quintessence}";
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

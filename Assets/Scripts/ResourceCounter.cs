using UnityEngine;
using TMPro;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] CampaignData campaignData;
    [SerializeField] TextMeshProUGUI aether;
    [SerializeField] TextMeshProUGUI brimstone;
    [SerializeField] TextMeshProUGUI quintessence;

    private void Start()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        aether.text = $": {campaignData.aether}";
        brimstone.text = $": {campaignData.brimstone}";
        quintessence.text = $": {campaignData.quintessence}";
    }

    private void Global_onUpdateResources()
    {
        UpdateDisplay();
    }

    private void OnEnable()
    {
        MissionEvents.i.onUpdateResources += Global_onUpdateResources;
    }

    private void OnDisable()
    {
        MissionEvents.i.onUpdateResources -= Global_onUpdateResources;
    }
}

using UnityEngine;
using TMPro;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] CampaignData campaignData;
    [SerializeField] TextMeshProUGUI aether;
    [SerializeField] TextMeshProUGUI brimstone;

    private void Start()
    {
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        aether.text = $"Aether: {campaignData.aether}";
        brimstone.text = $"Brimstone: {campaignData.brimstone}";
    }

    private void Global_onUpdateResources()
    {
        UpdateDisplay();
    }

    private void OnEnable()
    {
        GlobalEvents.i.onUpdateResources += Global_onUpdateResources;
    }

    private void OnDisable()
    {
        GlobalEvents.i.onUpdateResources -= Global_onUpdateResources;
    }
}

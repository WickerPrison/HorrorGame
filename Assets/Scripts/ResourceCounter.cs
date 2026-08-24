using UnityEngine;
using TMPro;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] CampaignData campaignData;

    private void Start()
    {
        UpdateDisplay(campaignData.aether);
    }

    void UpdateDisplay(int value)
    {
        text.text = $"Aether: {value}";
    }

    private void Global_onUpdateResources(int amount)
    {
        UpdateDisplay(amount);
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

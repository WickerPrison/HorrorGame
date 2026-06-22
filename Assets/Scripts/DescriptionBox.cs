using UnityEngine;
using TMPro;

public class DescriptionBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;

    private void OnEnable()
    {
        CampaignEvents.i.onSetDescription += Campaign_onSetDescription;
    }

    private void OnDisable()
    {
        CampaignEvents.i.onSetDescription -= Campaign_onSetDescription;
    }

    private void Campaign_onSetDescription(string description)
    {
        text.text = description;
    }
}

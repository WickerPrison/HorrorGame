using UnityEngine;

public class Recruits : MonoBehaviour
{
    [SerializeField] CampaignData campaignData;
    [SerializeField] RecruitsUnit[] recruitsUnits;

    void Start()
    {
        UpdateRecruitsUi();
    }

    public void UpdateRecruitsUi()
    {
        for (int i = 0; i < recruitsUnits.Length; i++)
        {
            if (i < campaignData.recruits.Count)
            {
                recruitsUnits[i].UpdateUnitUi(campaignData.recruits[i]);
            }
            else
            {
                recruitsUnits[i].UpdateUnitUi(null);
            }
        }
    }

    private void Campaign_onUpdateResources()
    {
        for(int i = 0; i < recruitsUnits.Length; i++)
        {
            recruitsUnits[i].UpdateBuyInteractivity();
        }
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

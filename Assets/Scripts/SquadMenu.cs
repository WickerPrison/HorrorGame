using UnityEngine;

public class SquadMenu : MonoBehaviour
{
    [SerializeField] CampaignData campaignData;
    [SerializeField] SquadUnit[] squadUnits;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < campaignData.squad.Length; i++)
        {
            squadUnits[i].SetUnitData(campaignData.squad[i]);
        }
    }
}

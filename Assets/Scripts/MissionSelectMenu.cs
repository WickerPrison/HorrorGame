using UnityEngine;

public class MissionSelectMenu : MonoBehaviour
{
    [SerializeField] CampaignData campaignData;
    [SerializeField] GameObject missionPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(campaignData.missions == null)
        {
            campaignData.ResetCampaignData();
        }
        foreach(LevelDetailsData mission in campaignData.missions)
        {
            Mission misisonObject = Instantiate(missionPrefab).GetComponent<Mission>();
            misisonObject.transform.SetParent(gameObject.transform);
            misisonObject.data = mission;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

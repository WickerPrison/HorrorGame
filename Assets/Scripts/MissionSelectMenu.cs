using UnityEngine;

public class MissionSelectMenu : MonoBehaviour
{
    [SerializeField] CampaignData campaignData;
    [SerializeField] GameObject missionPrefab;

    private void Awake()
    {
        if (campaignData.missions == null)
        {
            campaignData.ResetCampaignData();
        }
    }

    void Start()
    {
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

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
            Mission missionObject = Instantiate(missionPrefab).GetComponent<Mission>();
            missionObject.transform.SetParent(gameObject.transform);
            missionObject.transform.localScale = Vector3.one;
            missionObject.data = mission;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

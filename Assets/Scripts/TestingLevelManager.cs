using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingLevelManager : MonoBehaviour
{
    [SerializeField] CampaignData campaignData;

    private void OnEnable()
    {
        GlobalEvents.i.onUnitLeaveMission += Global_onUnitLeaveMission;
        PlayerEvents.i.onUnitDeath += Player_onUnitDeath;
    }

    private void OnDisable()
    {
        GlobalEvents.i.onUnitLeaveMission -= Global_onUnitLeaveMission;
        PlayerEvents.i.onUnitDeath -= Player_onUnitDeath;
    }

    private void Global_onUnitLeaveMission(PlayerUnit leavingUnit)
    {
        if(PlayerManager.i.AllUnitsCount() == 0)
        {
            if (campaignData.testingLevel)
            {
                SceneManager.LoadScene("PlaceholderMainMenu");
            }
            else
            {
                SceneManager.LoadScene("MissionSelect");
            }
        }
    }

    private void Player_onUnitDeath(PlayerUnit deadUnit)
    {
        if(PlayerManager.i.AllUnitsCount() == 0)
        {
            if (campaignData.testingLevel)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                SceneManager.LoadScene("MissionSelect");
            }
        }
    }
}

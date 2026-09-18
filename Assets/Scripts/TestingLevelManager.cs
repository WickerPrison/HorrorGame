using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingLevelManager : MonoBehaviour
{
    [SerializeField] CampaignData campaignData;

    private void OnEnable()
    {
        MissionEvents.i.onUnitLeaveMission += Mission_onUnitLeaveMission;
        PlayerEvents.i.onUnitDeath += Player_onUnitDeath;
    }

    private void OnDisable()
    {
        MissionEvents.i.onUnitLeaveMission -= Mission_onUnitLeaveMission;
        PlayerEvents.i.onUnitDeath -= Player_onUnitDeath;
    }

    private void Mission_onUnitLeaveMission(PlayerUnit leavingUnit)
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

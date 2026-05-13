using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingLevelManager : MonoBehaviour
{
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
            SceneManager.LoadScene("PlaceholderMainMenu");
        }
    }

    private void Player_onUnitDeath(PlayerUnit deadUnit)
    {
        if(PlayerManager.i.AllUnitsCount() == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

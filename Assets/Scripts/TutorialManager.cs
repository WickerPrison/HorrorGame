using UnityEngine;
using UnityEngine.SceneManagement;

public enum TutorialLevel
{
    ONE, TWO
}

public class TutorialManager : MonoBehaviour
{
    [SerializeField] TutorialLevel level;

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
        switch (level)
        {
            case TutorialLevel.ONE:
                SceneManager.LoadScene("Tutorial2");
                break;
        }
    }

    private void Player_onUnitDeath(PlayerUnit deadUnit)
    {
        switch (level)
        {
            case TutorialLevel.TWO:
                SceneManager.LoadScene("Tutorial2");
                break;
        }
    }
}

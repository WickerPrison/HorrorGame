using UnityEngine;
using UnityEngine.SceneManagement;

public enum TutorialLevel
{
    ONE, TWO, THREE
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
            case TutorialLevel.TWO:
                SceneManager.LoadScene("Tutorial3");
                break;
            case TutorialLevel.THREE:
                SceneManager.LoadScene("PlaceholderMainMenu");
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
            case TutorialLevel.THREE:
                SceneManager.LoadScene("Tutorial3");
                break;
        }
    }
}

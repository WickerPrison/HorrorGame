using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TutorialLevel
{
    ONE, TWO, THREE, FOUR
}

public class TutorialManager : MonoBehaviour
{
    [SerializeField] TutorialLevel level;
    [SerializeField] AbilityDictionary abilityDictionary;
    [SerializeField] TestPlayerUnitData tutorial1;
    [SerializeField] TestPlayerUnitData basic;
    [SerializeField] TestPlayerUnitData power;
    [SerializeField] TestPlayerUnitData testUnit1;
    [SerializeField] TestPlayerUnitData saint;
    [SerializeField] TestPlayerUnitData evil;


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

    private void Awake()
    {
        List<PlayerUnit> playerUnits = FindObjectsByType<PlayerUnit>(FindObjectsSortMode.None).ToList();
        switch (level)
        {
            case TutorialLevel.ONE:
                playerUnits[0].SetUnitData(new PlayerUnitData(abilityDictionary, tutorial1));
                break;
            case TutorialLevel.TWO:
                playerUnits[0].SetUnitData(new PlayerUnitData(abilityDictionary, basic));
                playerUnits[1].SetUnitData(new PlayerUnitData(abilityDictionary, power));
                break;
            case TutorialLevel.THREE:
                playerUnits[0].SetUnitData(new PlayerUnitData(abilityDictionary, testUnit1));
                playerUnits[1].SetUnitData(new PlayerUnitData(abilityDictionary, power));
                break;
            case TutorialLevel.FOUR:
                playerUnits[0].SetUnitData(new PlayerUnitData(abilityDictionary, saint));
                playerUnits[1].SetUnitData(new PlayerUnitData(abilityDictionary, power));
                playerUnits[2].SetUnitData(new PlayerUnitData(abilityDictionary, evil));
                break;
        }
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
                SceneManager.LoadScene("Tutorial4");
                break;
            case TutorialLevel.FOUR:
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
            case TutorialLevel.FOUR:
                SceneManager.LoadScene("Tutorial4");
                break;
        }
    }
}

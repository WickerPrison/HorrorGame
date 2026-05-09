using System.Collections.Generic;
using UnityEngine;

public class UnitsUi : MonoBehaviour
{
    [SerializeField] UnitStatUi[] unitUis;
    PlayerManager playerManager;

    private void Start()
    {
        playerManager = PlayerEvents.i.gameObject.GetComponent<PlayerManager>();
        UpdateUi();
    }

    void UpdateUi()
    {
        if (playerManager == null) return;
        for(int i = 0; i < 4; i++)
        {
            if(i < playerManager.allUnits.Count)
            {
                unitUis[i].SetUnit(playerManager.allUnits[i].data);
            }
            else
            {
                unitUis[i].SetUnit(null);
            }
        }
    }

    private void Player_onUnitStatChange(PlayerUnit changedUnit)
    {
        UpdateUi();
    }

    private void OnEnable()
    {
        PlayerEvents.i.onUnitStatChange += Player_onUnitStatChange;
    }

    private void OnDisable()
    {
        PlayerEvents.i.onUnitStatChange -= Player_onUnitStatChange;
    }
}

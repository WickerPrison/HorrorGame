using UnityEngine;

public class BarracksUnit : MonoBehaviour
{
    UnitStatUi statsUi;

    private void Awake()
    {
        statsUi = GetComponentInChildren<UnitStatUi>();
    }

    public void SetUnitData(PlayerUnitData unitData)
    {
        statsUi.SetUnit(unitData);
    }
}

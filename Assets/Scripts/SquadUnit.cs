using UnityEngine;

public class SquadUnit : MonoBehaviour
{
    UnitStatUi statsUi;
    [SerializeField] MenuButton remove;
    [SerializeField] MenuButton up;
    [SerializeField] MenuButton down;

    private void Awake()
    {
        statsUi = GetComponentInChildren<UnitStatUi>();
    }

    public void SetUnitData(PlayerUnitData unitData)
    {
        bool hasUnit = unitData != null;
        remove.gameObject.SetActive(hasUnit);
        up.gameObject.SetActive(hasUnit);
        down.gameObject.SetActive(hasUnit);
        if(hasUnit)
        {
            up.interactable = unitData.index != 0;
            down.interactable = unitData.index != 3;
        }
        statsUi.SetUnit(unitData);
    }
}

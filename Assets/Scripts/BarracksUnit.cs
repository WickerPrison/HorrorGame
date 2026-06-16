using UnityEngine;

public class BarracksUnit : MonoBehaviour
{
    UnitStatUi statsUi;
    [SerializeField] MenuButton assign;
    PlayerUnitData unit;
    Barracks barracks;

    private void Awake()
    {
        statsUi = GetComponentInChildren<UnitStatUi>();
    }

    private void Start()
    {
        barracks = GetComponentInParent<Barracks>();
    }

    public void SetUnitData(PlayerUnitData unitData, bool squadHasRoom)
    {
        assign.interactable = squadHasRoom;
        SetUnitData(unitData);
    }

    public void SetUnitData(PlayerUnitData unitData)
    {
        assign.gameObject.SetActive(unitData != null);
        unit = unitData;
        statsUi.SetUnit(unitData);
    }

    public void AssignToSquad()
    {
        barracks.AssignUnit(unit);
    }
}

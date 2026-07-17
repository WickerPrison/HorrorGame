using UnityEngine;

public class BarracksUnit : MonoBehaviour
{
    UnitStatUi statsUi;
    [SerializeField] MenuButton edit;
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
        edit.gameObject.SetActive(unitData != null);
        assign.gameObject.SetActive(unitData != null);
        unit = unitData;
        statsUi.SetUnit(unitData);
    }

    public void AssignToSquad()
    {
        barracks.AssignUnit(unit);
    }

    public void SelectUnit()
    {
        CampaignEvents.i.SelectUnit(unit);
    }
}

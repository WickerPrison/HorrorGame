using UnityEngine;

public class SquadUnit : MonoBehaviour
{
    UnitStatUi statsUi;
    [SerializeField] int index;
    [SerializeField] MenuButton edit;
    [SerializeField] MenuButton remove;
    [SerializeField] MenuButton up;
    [SerializeField] MenuButton down;
    SquadMenu squadMenu;

    private void Awake()
    {
        statsUi = GetComponentInChildren<UnitStatUi>();
        squadMenu = GetComponentInParent<SquadMenu>();
    }

    public void SetUnitData(PlayerUnitData unitData)
    {
        bool hasUnit = unitData.name != "";
        edit.gameObject.SetActive(hasUnit);
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

    public void SelectUnit()
    {
        squadMenu.SelectSquadUnit(index);
    }

    public void RemoveFromSquad()
    {
        squadMenu.RemoveFromSquad(index);
    }

    public void MoveUnitUp()
    {
        squadMenu.MoveUnitUp(index);
    }

    public void MoveUnitDown()
    {
        squadMenu.MoveUnitDown(index);
    }
}

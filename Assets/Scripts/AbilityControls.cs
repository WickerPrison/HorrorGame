using UnityEngine;

public class AbilityControls : MonoBehaviour
{
    [SerializeField] int index;
    [SerializeField] MenuButton removeAbility;
    [SerializeField] MenuButton abilityUp;
    [SerializeField] MenuButton abilityDown;
    SelectedUnit selectedUnit;

    private void Start()
    {
        selectedUnit = GetComponentInParent<SelectedUnit>();
    }

    public void SetUnitData(PlayerUnitData unitData)
    {
        if(unitData == null || unitData.abilities[index].type == AbilityType.NONE)
        {
            removeAbility.gameObject.SetActive(false);
            abilityUp.gameObject.SetActive(false);
            abilityDown.gameObject.SetActive(false);
        }
        else
        {
            removeAbility.gameObject.SetActive(true);
            abilityUp.gameObject.SetActive(true);
            abilityDown.gameObject.SetActive(true);
        }
    }

    public void MoveUp()
    {
        selectedUnit.MoveAbilityUp(index);
    }

    public void MoveDown()
    {
        selectedUnit.MoveAbilityDown(index);
    }

    public void UnequipAbility()
    {
        selectedUnit.UnequipAbility(index);
    }
}

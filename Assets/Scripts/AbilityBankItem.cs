using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityBankItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI abilityName;
    [SerializeField] MenuButton equipButton;
    [System.NonSerialized] public AbilityBank abilityBank;
    Ability currentAbility;

    public void SetAbility(Ability ability, PlayerUnitData selectedUnit)
    {
        currentAbility = ability;
        if(ability == null)
        {
            abilityName.text = "";
            equipButton.gameObject.SetActive(false);
        }
        else
        {
            abilityName.text = Utils.GetAbilityName(ability);
            bool showEquipButton = selectedUnit != null && selectedUnit.abilities.Any(ability => ability.type == AbilityType.NONE);
            equipButton.gameObject.SetActive(showEquipButton);
        }
    }

    public void EquipAbility()
    {
        abilityBank.EquipAbility(currentAbility);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentAbility == null || currentAbility.type == AbilityType.NONE) return;
        CampaignEvents.i.SetDescription(currentAbility.description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CampaignEvents.i.SetDescription("");
    }
}

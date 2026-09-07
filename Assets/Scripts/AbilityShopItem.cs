using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] TextMeshProUGUI abilityName;
    [SerializeField] Image aetherIcon;
    [SerializeField] MenuButton button;
    public AbilityShop abilityShop;
    Ability currentAbility;

    public void SetAbility(Ability ability)
    {
        currentAbility = ability;
        if (ability == null)
        {
            aetherIcon.enabled = false;
            abilityName.text = "";
            button.gameObject.SetActive(false);
        }
        else
        {
            aetherIcon.enabled = true;
            abilityName.text = Utils.GetAbilityName(ability);
            buttonText.text = $"Cost: {ability.cost}";
            button.interactable = CampaignManager.i.CanAfford(ability.cost);
        }
    }

    public void Buy()
    {
        abilityShop.BuyAbility(currentAbility);
    }

    public void BuyPermanentStock()
    {
        abilityShop.BuyAbilityDontRemove(currentAbility);
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

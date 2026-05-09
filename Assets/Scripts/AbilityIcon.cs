using UnityEngine;
using TMPro;

public class AbilityIcon : MonoBehaviour
{
    [SerializeField] AbilityDictionary abilityDictionary;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject icon;
    
    void Start()
    {
        Hide();
    }

    public void SetAbility(Ability ability, PlayerUnitData playerData)
    {
        if (ability == Ability.NONE)
        {
            Hide();
        }
        else
        {
            AbilityData data = abilityDictionary.abilityDict[ability];
            nameText.text = Utils.AppendUses(ability, data.abilityName, playerData);
            icon.SetActive(true);
            nameText.gameObject.SetActive(true);
        }
    }

    void Hide()
    {
        icon.SetActive(false);
        nameText.gameObject.SetActive(false);
    }
}

using UnityEngine;
using TMPro;

public class AbilityIcon : MonoBehaviour
{
    [SerializeField] AbilityDictionary abilityDictionary;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] GameObject icon;
    
    void Start()
    {
        Show(false);
    }

    public void SetAbility(Ability ability, PlayerUnitData playerData)
    {
        if (ability == Ability.NONE)
        {
            Show(false);
        }
        else
        {
            AbilityData data = abilityDictionary.abilityDict[ability];
            nameText.text = Utils.AppendUses(ability, data.abilityName, playerData);
            icon.SetActive(true);
            nameText.gameObject.SetActive(true);
        }
    }

    public void Show(bool show)
    {
        icon.SetActive(show);
        nameText.gameObject.SetActive(show);
    }
}

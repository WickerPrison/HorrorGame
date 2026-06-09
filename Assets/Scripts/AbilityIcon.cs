using UnityEngine;
using TMPro;

public class AbilityIcon : MonoBehaviour
{
    [SerializeField] AbilityDictionary abilityDictionary;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI buttonPrompt;
    [SerializeField] GameObject icon;
    
    void Start()
    {
        Show(false);
    }

    public void SetAbility(Ability ability)
    {
        if (ability.type == AbilityType.NONE)
        {
            Show(false);
        }
        else
        {
            nameText.text = $"{ability.abilityName} ({ability.uses}/{ability.maxUses})";
            icon.SetActive(true);
            nameText.gameObject.SetActive(true);
        }
    }

    public void Show(bool show)
    {
        icon.SetActive(show);
        nameText.gameObject.SetActive(show);
    }

    public void SetTexts(string abilityName, string button)
    {
        nameText.text = abilityName;
        buttonPrompt.text = button;
    }
}

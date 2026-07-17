using UnityEngine;
using TMPro;

public class UnitStatUi : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI morality;
    [SerializeField] TextMeshProUGUI health;
    [SerializeField] TextMeshProUGUI abilitiesHeader;
    [SerializeField] TextMeshProUGUI[] abilityNames;
    [SerializeField] AbilityDictionary abilityDict;

    private void Start()
    {
        SetUnit(null);
    }

    public void SetUnit(PlayerUnitData data)
    {
        if (data != null && data.name != "")
        {
            nameText.text = data.name;
            morality.text = $"Morality: {data.morality}";
            health.text = $"Health: {data.health}/{data.maxHealth}";
            abilitiesHeader.text = "Abilities";
            for(int i = 0; i < 4; i++)
            {
                abilityNames[i].text = Utils.GetAbilityName(data.abilities[i]);
            }
        }
        else
        {
            nameText.text = "";
            morality.text = "";
            health.text = "";
            abilitiesHeader.text = "";
            foreach(TextMeshProUGUI text in abilityNames)
            {
                text.text = "";
            }
        }
    }

    public void SetUnitWithAbilityDescription(PlayerUnitData data, int descriptionFontSize = 0)
    {
        SetUnit(data);
        if (data == null) return;
        string font1 = descriptionFontSize == 0 ? "" : $"<size={descriptionFontSize}>";
        string font2 = descriptionFontSize == 0 ? "" : "</size>";
        for (int i = 0; i < 4; i++)
        {
            if (data.abilities[i].type != AbilityType.NONE)
            {
                abilityNames[i].text += $" - {font1}{data.abilities[i].description}{font2}";
            }
        }
    }
}

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
        if(data != null)
        {
            nameText.text = data.name;
            morality.text = $"Morality: {data.morality}";
            health.text = $"Health: {data.health}/{data.maxHealth}";
            abilitiesHeader.text = "Abilities";
            for(int i = 0; i < 4; i++)
            {
                if(data.abilities[i].type != AbilityType.NONE)
                {
                    abilityNames[i].text = $"{data.abilities[i].abilityName} ({data.abilities[i].uses}/{data.abilities[i].maxUses})";
                }
                else
                {
                    abilityNames[i].text = "";
                }
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
}

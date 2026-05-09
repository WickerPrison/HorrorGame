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
                if(data.abilities[i] != Ability.NONE)
                {
                    string text = abilityDict.abilityDict[data.abilities[i]].abilityName;
                    abilityNames[i].text = Utils.AppendUses(data.abilities[i], text, data);
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

using UnityEngine;

public class Ability
{
    public AbilityType type;
    public string abilityName;
    public int maxUses;
    public int uses;

    public Ability(AbilityDictionary dictionary, AbilityType abilityType)
    {
        AbilityData data = dictionary.abilityDict[type];
        type = abilityType;
        abilityName = data.abilityName;
        maxUses = data.uses;
        uses = data.uses;
    }

    public Ability(AbilityData data)
    {
        type = data.type;
        abilityName = data.abilityName;
        maxUses = data.uses;
        uses = data.uses;
    }

    public Ability()
    {
        type = AbilityType.NONE;
        abilityName = "none";
        maxUses = 0;
        uses = 0;
    }
}

using UnityEngine;

public class Ability
{
    public AbilityType type;
    public string abilityName;
    public string description;
    public int maxUses;
    public int uses;

    public Ability(AbilityDictionary dictionary, AbilityType abilityType) : this(dictionary.abilityDict[abilityType])
    {

    }

    public Ability(AbilityData data)
    {
        type = data.type;
        abilityName = data.abilityName;
        description = data.description;
        maxUses = data.uses;
        uses = data.uses;
    }

    public Ability()
    {
        type = AbilityType.NONE;
        abilityName = "none";
        description = "";
        maxUses = 0;
        uses = 0;
    }

    public static Ability None()
    {
        return new Ability();
    }
}

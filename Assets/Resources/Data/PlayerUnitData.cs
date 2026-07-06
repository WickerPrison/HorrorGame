using System.Linq;
using UnityEngine;

public class PlayerUnitData
{
    public string name;
    public int morality;
    public int health;
    public int maxHealth;
    public int index;
    public Ability[] abilities;
    public int cost;

    public PlayerUnitData(AbilityDictionary dictionary, TestPlayerUnitData testPlayerUnitData) : this
        (
            testPlayerUnitData.unitName,
            testPlayerUnitData.maxHealth,
            testPlayerUnitData.index,
            testPlayerUnitData.morality,
            testPlayerUnitData.abilities.Select(a => new Ability(dictionary, a)).ToArray()
        )
    { }

    public PlayerUnitData(string unitName, int unitMaxHealth, int unitIndex = 0, int unitMorality = 0, Ability[] unitAbilities = null)
    {
        name = unitName;
        maxHealth = unitMaxHealth;
        health = maxHealth;
        index = unitIndex;
        morality = unitMorality;
        if(unitAbilities == null)
        {
            abilities = new Ability[] { new Ability(), new Ability(), new Ability(), new Ability() };
        }
        else
        {
            abilities = unitAbilities;
        }
    }

    // returns -1 if ability has no usage limit
    // returns 0 if unit does not have ability
    public int UsesOfAbilityType(AbilityType abilityType)
    {
        foreach(Ability ability in abilities)
        {
            if(ability.type == abilityType)
            {
                if(ability.uses > 0)
                {
                    return ability.uses;
                }
                if(ability.maxUses == -1)
                {
                    return -1;
                }
            }
        }
        return 0;
    }

    public bool CanUseAbilityType(AbilityType abilityType)
    {
        int uses = UsesOfAbilityType(abilityType);
        if (uses > 0 || uses == -1) return true;
        return false;
    }

    //TODO: write test for this
    public void GainUsesOfAbilityType(AbilityType abilityType, int amount)
    {
        foreach(Ability ability in abilities)
        {
            if(ability.type == abilityType && ability.uses < ability.maxUses)
            {
                int diff = ability.maxUses - ability.uses;
                if(diff >= amount)
                {
                    ability.uses += amount;
                    return;
                }
                else
                {
                    ability.uses = ability.maxUses;
                    amount -= diff;
                }
            }
        }
    }
}

using UnityEngine;

public class PlayerUnitData
{
    public string name;
    public int morality;
    public int health;
    public int maxHealth;
    public int index;
    public Ability[] abilities;

    public int mineUses = 2;

    public PlayerUnitData(string unitName, int unitMaxHealth, int unitIndex, int unitMorality = 0, Ability[] unitAbilities = null)
    {
        name = unitName;
        maxHealth = unitMaxHealth;
        health = maxHealth;
        index = unitIndex;
        morality = unitMorality;
        if(unitAbilities == null)
        {
            abilities = new Ability[] { Ability.NONE, Ability.NONE, Ability.NONE, Ability.NONE };
        }
        else
        {
            abilities = unitAbilities;
        }
    }
}

using UnityEngine;

public class PlayerUnitData
{
    public string name;
    public int morality;
    public int health;
    public int maxHealth;
    public Ability[] abilities;

    public int mineUses = 2;

    public PlayerUnitData(string unitName, int unitMaxHealth)
    {
        name = unitName;
        maxHealth = unitMaxHealth;
        health = maxHealth;
    }
}

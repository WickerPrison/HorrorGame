using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/TestPlayerUnitData")]
public class TestPlayerUnitData : ScriptableObject
{
    public bool isActive;
    public string unitName;
    public int morality;
    public int health;
    public int maxHealth;
    public int index;
    public AbilityType[] abilities;
}

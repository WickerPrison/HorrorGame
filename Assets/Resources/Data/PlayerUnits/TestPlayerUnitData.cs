using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/TestPlayerUnitData")]
public class TestPlayerUnitData : ScriptableObject
{
    public string unitName;
    public int morality;
    public int health;
    public int maxHealth;
    public Ability[] abilities;
}

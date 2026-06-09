using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/AbilityData")]
public class AbilityData : ScriptableObject
{
    public AbilityType type;
    public string abilityName;
    public int uses;
}

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDictionary", menuName = "Scriptable Objects/AbilityDictionary")]
public class AbilityDictionary : ScriptableObject
{
    [SerializeField] AbilityData collect;
    [SerializeField] AbilityData power;
    [SerializeField] AbilityData scan;
    [SerializeField] AbilityData mine;


    Dictionary<Ability, AbilityData> _abilityDict;
    public Dictionary<Ability, AbilityData> abilityDict
    {
        get
        {
            if(_abilityDict == null)
            {
                _abilityDict = GetDictionary();
            }
            return _abilityDict;
        }
    }

    Dictionary<Ability, AbilityData> GetDictionary()
    {
        return new Dictionary<Ability, AbilityData>()
        {
            { Ability.COLLECT, collect },
            { Ability.POWER, power },
            { Ability.SCAN, scan },
            { Ability.MINE, mine },
        };
    }
}

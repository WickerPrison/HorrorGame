using System.Collections.Generic;
using UnityEngine;

public enum AbilityType
{
    NONE, COLLECT, SCAN, POWER, MINE, SANCTIFY, DESECRATE, CAMERA
}

[CreateAssetMenu(fileName = "AbilityDictionary", menuName = "Scriptable Objects/AbilityDictionary")]
public class AbilityDictionary : ScriptableObject
{
    [SerializeField] AbilityData collect;
    [SerializeField] AbilityData power;
    [SerializeField] AbilityData scan;
    [SerializeField] AbilityData mine;
    [SerializeField] AbilityData sanctify;
    [SerializeField] AbilityData desecrate;
    [SerializeField] AbilityData camera;


    Dictionary<AbilityType, AbilityData> _abilityDict;
    public Dictionary<AbilityType, AbilityData> abilityDict
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

    Dictionary<AbilityType, AbilityData> GetDictionary()
    {
        return new Dictionary<AbilityType, AbilityData>()
        {
            { AbilityType.COLLECT, collect },
            { AbilityType.POWER, power },
            { AbilityType.SCAN, scan },
            { AbilityType.MINE, mine },
            { AbilityType.SANCTIFY, sanctify },
            { AbilityType.DESECRATE, desecrate },
            { AbilityType.CAMERA, camera },
        };
    }
}

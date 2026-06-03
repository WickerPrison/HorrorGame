using UnityEngine;

public enum ThreatLevel
{
    LOW, MEDIUM, HIGH
}

public enum Interference
{
    LOW, MEDIUM, HIGH
}

public enum Rewards
{
    LOW, MEDIUM, HIGH
}

[CreateAssetMenu(fileName = "LevelDetails", menuName = "Scriptable Objects/LevelDetails")]
public class LevelDetails : ScriptableObject
{
    public ThreatLevel threatLevel;
    public Interference interference;
    public Rewards rewards;
}

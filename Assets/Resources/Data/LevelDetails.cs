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

public enum Openness
{
    LOW, MEDIUM, HIGH
}

[CreateAssetMenu(fileName = "LevelDetails", menuName = "Scriptable Objects/LevelDetails")]
public class LevelDetails : ScriptableObject
{
    public ThreatLevel threatLevel;
    public Interference interference;
    public Rewards rewards;
    public Openness openness;

    public void SetData(LevelDetailsData data)
    {
        threatLevel = data.threatLevel;
        interference = data.interference;
        rewards = data.rewards;
        openness = data.openness;
    }
}

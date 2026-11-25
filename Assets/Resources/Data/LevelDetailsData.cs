using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelDetailsData
{
    public string missionName;
    public ThreatLevel threatLevel;
    public List<EnemyTypes> enemyTypes;
    public Interference interference;
    public Rewards rewards;
    public Openness openness;
    public int cost;

    static Dictionary<ThreatLevel, List<EnemyTypes>> threatLevelTypes = new Dictionary<ThreatLevel, List<EnemyTypes>>
    {
        { ThreatLevel.LOW, new List<EnemyTypes> { EnemyTypes.IMP } },
        { ThreatLevel.MEDIUM, new List<EnemyTypes> { EnemyTypes.IMP, EnemyTypes.DEVIL_SKULL, EnemyTypes.BEHEMOTH } },
        { ThreatLevel.HIGH, new List<EnemyTypes> { EnemyTypes.IMP, EnemyTypes.DEVIL_SKULL, EnemyTypes.BEHEMOTH, EnemyTypes.DEMON } },
    };

    private static readonly string AlphanumericChars =
    "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";


    public void GenerateData()
    {
        missionName = GenerateOperationName();
        int randInt = Random.Range(0, 3);
        threatLevel = randInt switch
        {
            0 => ThreatLevel.LOW,
            1 => ThreatLevel.MEDIUM,
            2 => ThreatLevel.HIGH,
        };
        enemyTypes = ChooseEnemyTypes(threatLevel);
        randInt = Random.Range(0, 3);
        interference = randInt switch
        {
            0 => Interference.LOW,
            1 => Interference.MEDIUM,
            2 => Interference.HIGH,
        };
        randInt = Random.Range(0, 3);
        rewards = randInt switch
        {
            0 => Rewards.LOW,
            1 => Rewards.MEDIUM,
            2 => Rewards.HIGH,
        };
        randInt = Random.Range(0, 3);
        openness = randInt switch
        {
            0 => Openness.LOW,
            1 => Openness.MEDIUM,
            2 => Openness.HIGH,
        };

        cost = Random.Range(1, 3);
    }

    public string GenerateOperationName()
    {
        System.Random random = new System.Random();

        string part1 = GetRandomAlphanumericString(4, random);
        string part2 = GetRandomAlphanumericString(4, random);

        return $"Operation {part1}-{part2}";
    }

    public static List<EnemyTypes> ChooseEnemyTypes(ThreatLevel threatLevel)
    {
        List<EnemyTypes> enemyTypes = new List<EnemyTypes>();
        (int, int) typeRange = threatLevel switch
        {
            ThreatLevel.LOW => (1, 1),
            ThreatLevel.MEDIUM => (1, 3),
            ThreatLevel.HIGH => (2, 4),
        };
        int typeCount = Random.Range(typeRange.Item1, typeRange.Item2 + 1);
        List<EnemyTypes> enemyOptions = new List<EnemyTypes>(threatLevelTypes[threatLevel]);
        for(int i = 0; i < typeCount; i++)
        {
            int randInt = Random.Range(0, enemyOptions.Count);
            enemyTypes.Add(enemyOptions[randInt]);
            enemyOptions.RemoveAt(randInt);
        }
        return enemyTypes;
    }

    static string GetRandomAlphanumericString(int length, System.Random random)
    {
        char[] result = new char[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = AlphanumericChars[random.Next(AlphanumericChars.Length)];
        }

        return new string(result);
    }

    static string ColoredString(string text, Color color)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
    }

    public static string GetString(ThreatLevel threatLevel, ColorData colorData)
    {
        return threatLevel switch
        {
            ThreatLevel.LOW => ColoredString("Low", colorData.safe),
            ThreatLevel.MEDIUM => ColoredString("Medium", colorData.unscannable),
            ThreatLevel.HIGH => ColoredString("High", colorData.danger),
        };
    }

    public static string GetString(Interference interference, ColorData colorData)
    {
        return interference switch
        {
            Interference.LOW => ColoredString("Low", colorData.safe),
            Interference.MEDIUM => ColoredString("Medium", colorData.unscannable),
            Interference.HIGH => ColoredString("High", colorData.danger),
        };
    }

    public static string GetString(Rewards rewards)
    {
        return rewards switch
        {
            Rewards.LOW => "Low",
            Rewards.MEDIUM => "Medium",
            Rewards.HIGH => "High",
        };
    }
}

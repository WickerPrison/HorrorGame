using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils
{
    private static readonly string AlphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static List<Room> GetRooms(Vector3 position, float radius = 0.5f)
    {
        List<Room> rooms = Physics2D.OverlapCircleAll(position, radius)
            .Where(c => c.isTrigger)
            .Select(c => c.GetComponent<Room>())
            .Where(room => room != null)
            .ToList();
        return rooms;
    }

    public static Room GetRoom(Vector3 position, float radius = 0.2f)
    {
        List<Room> rooms = GetRooms(position, radius);
        return rooms.Count > 0 ? rooms[0] : null;
    }

    public static string GetAbilityName(Ability ability)
    {
        if (ability.type != AbilityType.NONE)
        {
            if (ability.maxUses == -1)
            {
               return ability.abilityName;
            }
            else
            {
               return $"{ability.abilityName} ({ability.uses}/{ability.maxUses})";
            }
        }
        else
        {
           return "";
        }
    }

    public static string GetRandomAlphanumericString(int length)
    {
        System.Random random = new System.Random();
        return GetRandomAlphanumericString(length, random);
    }

    public static string GetRandomAlphanumericString(int length, System.Random random)
    {
        char[] result = new char[length];

        for (int i = 0; i < length; i++)
        {
            result[i] = AlphanumericChars[random.Next(AlphanumericChars.Length)];
        }

        return new string(result);
    }
}

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

    public static void IncorrectInitialization(string className)
    {
        throw new System.Exception($"{className} must be initialized with constructor method");
    }

    public static Quaternion RotateFacingDirection(Vector3 currentDirection, float angleDegrees)
    {
        if (currentDirection == Vector3.zero)
            return Quaternion.identity;

        float currentAngle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;
        float targetAngle = currentAngle + angleDegrees;

        return Quaternion.Euler(0f, 0f, targetAngle);
    }

    public static Vector3 RotateDirection(Vector3 oldDirection, float degrees)
    {
        Vector3 newDirection = Vector3.zero;
        newDirection.x = Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.x - Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.y;
        newDirection.y = Mathf.Sin(degrees * Mathf.Deg2Rad) * oldDirection.x + Mathf.Cos(degrees * Mathf.Deg2Rad) * oldDirection.y;
        return newDirection;
    }

    public static Quaternion Look(Vector2 lookDir)
    {
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public static float GetAngle(Vector3 vectorA, Vector3 vectorB)
    {
        float angle = Mathf.Acos(Vector3.Dot(vectorA, vectorB) / (vectorA.magnitude * vectorB.magnitude));
        return angle * Mathf.Rad2Deg;
    }
}

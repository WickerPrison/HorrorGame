using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils
{
    public static List<Room> GetRooms(Vector3 position, float radius = 0.5f)
    {
        List<Room> rooms = Physics2D.OverlapCircleAll(position, radius)
            .Where(c => c.isTrigger)
            .Select(c => c.GetComponent<Room>())
            .Where(room => room != null)
            .ToList();
        return rooms;
    }

    public static Room GetRoom(Vector3 position, float radius = 0.5f)
    {
        return GetRooms(position, radius)[0];
    }
}

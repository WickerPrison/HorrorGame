using System.Collections.Generic;
using UnityEngine;

public static class TestingUtils
{
    public static void GiveRoomsVisionNodes()
    {
        List<Room> rooms = Utils.GetRooms(Vector3.zero, 100);
        foreach(Room room in rooms)
        {
            RoomTestingUtils roomTestingUtils = room.GetComponent<RoomTestingUtils>();
            if (roomTestingUtils == null)
            {
                roomTestingUtils = room.gameObject.AddComponent<RoomTestingUtils>();
            }

            roomTestingUtils.SpawnVisionNode();
        }
    }
}

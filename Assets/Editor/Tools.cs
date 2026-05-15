using UnityEngine;
using UnityEditor;

public static class Tools
{
    [MenuItem("Tools/Rebuild All Rooms _&R")]
    public static void RebuildRooms()
    {
        RoomSetup[] rooms = GameObject.FindObjectsByType<RoomSetup>(FindObjectsSortMode.None);
        foreach(RoomSetup room in rooms)
        {
            room.BuildRooms();
        }
    }
}

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class Tools
{
    static float nodeSize = 0.25f;

    static RoomSetup[] rooms
    {
        get
        {
            return GameObject.FindObjectsByType<RoomSetup>(FindObjectsSortMode.None);
        }
    }

    static LevelObjectSetup[] levelObjects
    {
        get
        {
            return GameObject.FindObjectsByType<LevelObjectSetup>(FindObjectsSortMode.None);
        }
    }

    [MenuItem("Tools/Rebuild All Rooms _&L")]
    public static void RebuildRooms()
    {
        foreach(RoomSetup room in rooms)
        {
            room.BuildRooms();
        }
    }

    [MenuItem("Tools/Move Everything/Left %&LEFT")]
    public static void MoveAllRoomsLeft()
    {
        Move(new Vector3(-nodeSize, 0, 0));
    }

    [MenuItem("Tools/Move Everything/Right %&RIGHT")]
    public static void MoveAllRoomsRight()
    {
        Move(new Vector3(nodeSize, 0, 0));
    }

    [MenuItem("Tools/Move Everything/Up %&UP")]
    public static void MoveAllRoomsUp()
    {
        Move(new Vector3(0, nodeSize, 0));
    }

    [MenuItem("Tools/Move Everything/Down %&DOWN")]
    public static void MoveAllRoomsDown()
    {
        Move(new Vector3(0, -nodeSize, 0));
    }

    static void Move(Vector3 direction)
    {
        Undo.IncrementCurrentGroup();
        Undo.SetCurrentGroupName("Move Everything");

        foreach (RoomSetup room in rooms)
        {
            room.MoveRoom(direction);
        }

        Undo.RegisterCompleteObjectUndo(levelObjects, "Move Everything");
        foreach (LevelObjectSetup levelObject in levelObjects)
        {
            Undo.RegisterCompleteObjectUndo(levelObject.gameObject, "Move Everything");
        }

        foreach (LevelObjectSetup levelObject in levelObjects)
        {
            levelObject.MoveObject(direction);
        }

        foreach (LevelObjectSetup levelObject in levelObjects)
        {
            EditorUtility.SetDirty(levelObject);
            EditorUtility.SetDirty(levelObject.gameObject);
        }

        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
}

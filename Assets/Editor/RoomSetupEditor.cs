using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(RoomSetup))]
public class RoomSetupEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        RoomSetup roomSetup = (RoomSetup)target;

        if (GUILayout.Button("Build Room"))
        {
            roomSetup.BuildRooms();
        }

        DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        RoomSetup roomSetup = (RoomSetup)target;

        Handles.color = Color.cyan;

        for(int i = 0; i < roomSetup.handles.Count; i++)
        {
            Vector3 handle = roomSetup.handles[i];
            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.FreeMoveHandle(handle, 0.2f, Vector3.one * 5f, Handles.SphereHandleCap);
            float xPos = Mathf.Round((newPos.x - roomSetup.gridCenter.x) / roomSetup.nodeSize) * roomSetup.nodeSize + roomSetup.gridCenter.x + roomSetup.nodeSize / 2;
            float yPos = Mathf.Round((newPos.y - roomSetup.gridCenter.y) / roomSetup.nodeSize) * roomSetup.nodeSize + roomSetup.gridCenter.y + roomSetup.nodeSize / 2;
            newPos = new Vector3(xPos, yPos, 0);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(roomSetup, "Move Room Setup Handle");
                roomSetup.handles[i] = newPos;
                Vector3 xLockedPos = roomSetup.handles[roomSetup.xLocks[i]];
                roomSetup.handles[roomSetup.xLocks[i]] = new Vector3(newPos.x, xLockedPos.y, 0);
                Vector3 yLockedPos = roomSetup.handles[roomSetup.yLocks[i]];
                roomSetup.handles[roomSetup.yLocks[i]] = new Vector3(yLockedPos.x, newPos.y, 0);
                EditorUtility.SetDirty(roomSetup);
            }
        }
    }
}

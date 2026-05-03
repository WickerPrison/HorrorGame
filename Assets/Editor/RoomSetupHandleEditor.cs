using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomSetupHandle))]
public class RoomSetupHandleEditor : Editor
{
    private void OnSceneGUI()
    {
        RoomSetupHandle handle = (RoomSetupHandle)target;

        Handles.color = Color.cyan;

        EditorGUI.BeginChangeCheck();
        Vector3 newPoint = Handles.FreeMoveHandle(handle.transform.position, 0.5f, Vector3.one * 0.2f, Handles.SphereHandleCap);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(handle.transform, "Move Room Setup Handle");
            handle.transform.position = newPoint;
        }
    }
}

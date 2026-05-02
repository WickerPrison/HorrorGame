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
            Undo.RegisterCompleteObjectUndo(roomSetup, "Build Room");
            Undo.RegisterCompleteObjectUndo(roomSetup.gameObject, "Build Room");
            BoxCollider2D collider = roomSetup.GetComponent<BoxCollider2D>();
            Undo.RegisterCompleteObjectUndo(collider, "Build Room");

            RoomSetup(roomSetup, collider);
            ClearWalls(roomSetup);
            BuildWalls(roomSetup, 0, 1);
            BuildWalls(roomSetup, 0, 2);
            BuildWalls(roomSetup, 1, 3);
            BuildWalls(roomSetup, 2, 3);
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

    void RoomSetup(RoomSetup roomSetup, BoxCollider2D collider)
    {
        float xPos = (roomSetup.handles[0].x + roomSetup.handles[1].x) / 2f;
        float yPos = (roomSetup.handles[0].y + roomSetup.handles[2].y) / 2f;
        roomSetup.transform.position = new Vector3(xPos, yPos, 0);
        float xWidth = Vector3.Distance(roomSetup.handles[0], roomSetup.handles[1]) - roomSetup.wallWidth;
        float yWidth = Vector3.Distance(roomSetup.handles[0], roomSetup.handles[2]) - roomSetup.wallWidth;
        collider.size = new Vector2(xWidth, yWidth);
    }

    void ClearWalls(RoomSetup roomSetup)
    {
        for(int i = roomSetup.wallsList.Count - 1; i >= 0; i--)
        {
            GameObject wall = roomSetup.wallsList[i];
            if(wall != null)
            {
                roomSetup.wallsList.RemoveAt(i);
                Undo.DestroyObjectImmediate(wall);
            }
        }
    }

    void BuildWalls(RoomSetup roomSetup, int index1, int index2)
    {
        List<Vector3> points = new List<Vector3>();
        points.Add(roomSetup.handles[index1]);

        RaycastHit2D[] hits = Physics2D.LinecastAll(roomSetup.handles[index1], roomSetup.handles[index2], roomSetup.doorDetectionMask);
        if (Mathf.Abs(roomSetup.handles[index2].x - roomSetup.handles[index1].x) > Mathf.Abs(roomSetup.handles[index2].y - roomSetup.handles[index1].y))
        {
            HorizontalDoorPoints(hits, points);
        }

        points.Add(roomSetup.handles[index2]);

        for(int i = 0; i < points.Count; i += 2)
        {
            CreateWall(roomSetup, points[i], points[i + 1]);
        }
    }

    void HorizontalDoorPoints(RaycastHit2D[] hits, List<Vector3> points)
    {
        foreach(RaycastHit2D hit in hits)
        {
            float width = hit.collider.transform.localScale.x;
            float xPos = hit.transform.position.x;
            points.Add(new Vector3(xPos + width / 2, points[0].y, 0));
            points.Add(new Vector3(xPos - width / 2, points[0].y, 0));
        }
    }

    void CreateWall(RoomSetup roomSetup, Vector3 point1, Vector3 point2)
    {
        float distance = Vector3.Distance(point1, point2);
        Vector3 direction = Vector3.Normalize(point2 - point1);
        Vector3 midPoint = (point1 + point2) / 2;

        GameObject newWall = (GameObject)PrefabUtility.InstantiatePrefab(roomSetup.wallPrefab);
        Undo.RegisterCreatedObjectUndo(newWall, "Create wall");
        newWall.transform.SetParent(roomSetup.wallsParent.transform);
        newWall.transform.position = midPoint;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            newWall.transform.localScale = new Vector3(distance + roomSetup.wallWidth, roomSetup.wallWidth, 1);
        }
        else
        {
            newWall.transform.localScale = new Vector3(roomSetup.wallWidth, distance + roomSetup.wallWidth, 1);
        }
        roomSetup.wallsList.Add(newWall);
    }
}

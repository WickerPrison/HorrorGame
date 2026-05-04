using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomSetup : MonoBehaviour
{
    public Vector3 gridCenter;
    public float nodeSize = 0.25f;
    public float wallWidth = 0.1f;
    public List<Vector3> handles = new List<Vector3>()
    {
        new Vector3(1, 1, 0), // top right
        new Vector3(-1, 1, 0), // top left
        new Vector3(1, -1, 0), // bottom right
        new Vector3(-1, -1, 0), // bottom left
    };
    public int[] xLocks = { 2, 3, 0, 1 };
    public int[] yLocks = { 1, 0, 3, 2 };
    public GameObject wallsParent;
    public GameObject wallPrefab;
    public LayerMask doorDetectionMask;

#if UNITY_EDITOR
    public void BuildRooms()
    {
        Undo.RegisterCompleteObjectUndo(this, "Build Room");
        Undo.RegisterCompleteObjectUndo(gameObject, "Build Room");
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        Undo.RegisterCompleteObjectUndo(collider, "Build Room");

        Setup(collider);
        ClearWalls();
        BuildWalls(0, 1);
        BuildWalls(0, 2);
        BuildWalls(1, 3);
        BuildWalls(2, 3);
    }

    void Setup(BoxCollider2D collider)
    {
        float xPos = (handles[0].x + handles[1].x) / 2f;
        float yPos = (handles[0].y + handles[2].y) / 2f;
        transform.position = new Vector3(xPos, yPos, 0);
        float xWidth = Vector3.Distance(handles[0], handles[1]) - wallWidth;
        float yWidth = Vector3.Distance(handles[0], handles[2]) - wallWidth;
        collider.size = new Vector2(xWidth, yWidth);
    }

    void ClearWalls()
    {
        Transform parent = wallsParent.transform;
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            GameObject wall = parent.GetChild(i).gameObject;
            if (wall != null)
            {
                Undo.DestroyObjectImmediate(wall);
            }
        }
    }

    void BuildWalls(int index1, int index2)
    {
        List<Vector3> points = new List<Vector3>();
        points.Add(handles[index1]);

        RaycastHit2D[] hits = Physics2D.LinecastAll(handles[index1], handles[index2], doorDetectionMask);
        if (Mathf.Abs(handles[index2].x - handles[index1].x) > Mathf.Abs(handles[index2].y - handles[index1].y))
        {
            HorizontalDoorPoints(hits, points);
        }
        else
        {
            VerticalDoorPoints(hits, points);
        }

        points.Add(handles[index2]);

        for (int i = 0; i < points.Count; i += 2)
        {
            CreateWall(points[i], points[i + 1]);
        }
    }

    void HorizontalDoorPoints(RaycastHit2D[] hits, List<Vector3> points)
    {
        foreach (RaycastHit2D hit in hits)
        {
            float width = hit.collider.transform.localScale.x;
            float xPos = hit.transform.position.x;
            points.Add(new Vector3(xPos + width / 2, points[0].y, 0));
            points.Add(new Vector3(xPos - width / 2, points[0].y, 0));
        }
    }

    void VerticalDoorPoints(RaycastHit2D[] hits, List<Vector3> points)
    {
        foreach (RaycastHit2D hit in hits)
        {
            float width = hit.collider.transform.localScale.x;
            float yPos = hit.transform.position.y;
            points.Add(new Vector3(points[0].x, yPos + width / 2, 0));
            points.Add(new Vector3(points[0].x, yPos - width / 2, 0));
        }
    }

    void CreateWall(Vector3 point1, Vector3 point2)
    {
        float distance = Vector3.Distance(point1, point2);
        Vector3 direction = Vector3.Normalize(point2 - point1);
        Vector3 midPoint = (point1 + point2) / 2;

        GameObject newWall = (GameObject)PrefabUtility.InstantiatePrefab(wallPrefab);
        Undo.RegisterCreatedObjectUndo(newWall, "Create wall");
        newWall.transform.SetParent(wallsParent.transform);
        newWall.transform.position = midPoint;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            newWall.transform.localScale = new Vector3(distance + wallWidth, wallWidth, 1);
        }
        else
        {
            newWall.transform.localScale = new Vector3(wallWidth, distance + wallWidth, 1);
        }
    }
#endif
}

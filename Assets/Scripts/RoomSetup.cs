using System.Collections.Generic;
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
    public List<GameObject> wallsList = new List<GameObject>();
    public LayerMask doorDetectionMask;
}

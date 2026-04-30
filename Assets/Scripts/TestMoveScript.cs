using Pathfinding;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AIPath))]
public class TestMoveScript : MonoBehaviour
{
    private Seeker seeker;
    private AIPath aiPath;

    void Awake()
    {
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
    }

    void Update()
    {
        // New Input System way to detect left-click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Get mouse position in world space
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = 0f;   // keep everything in 2D plane

            // Force a new path (works reliably in free A* 4.2.17)
            seeker.StartPath(transform.position, mouseWorldPos, OnPathComplete);

            // Tell AIPath where to go
            aiPath.destination = mouseWorldPos;

            Debug.Log("Clicked destination: " + mouseWorldPos);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
            Debug.Log("Path calculated! Length: " + p.vectorPath.Count);
        else
            Debug.LogError("Path failed: " + p.errorLog);
    }
}

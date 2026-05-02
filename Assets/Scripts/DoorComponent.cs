using Pathfinding;
using UnityEngine;

public class DoorComponent : MonoBehaviour
{
    [SerializeField] Vector3 openPosition;
    [SerializeField] Vector3 closedPosition;
    [SerializeField] Vector3 openScale;
    [SerializeField] Vector3 closedScale;
    [SerializeField] Door door;
    Collider2D myCollider;

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        door.onDoorProgress += Door_onDoorProgress;
    }

    private void OnDisable()
    {
        door.onDoorProgress -= Door_onDoorProgress;
    }

    private void Door_onDoorProgress(float progress)
    {
        transform.localPosition = Vector3.Lerp(closedPosition, openPosition, progress);
        transform.localScale = Vector3.Lerp(closedScale, openScale, progress);

        Bounds bounds = myCollider.bounds;
        bounds.Expand(new Vector3(0, 0, 50f));
        GraphUpdateObject graphObject = new GraphUpdateObject(bounds)
        {
            updatePhysics = true
        };
        AstarPath.active.UpdateGraphs(graphObject);
    }
}

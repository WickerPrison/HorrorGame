using UnityEngine;

public class DoorSetup : MonoBehaviour
{
    public Vector3 gridCenter;
    public float nodeSize = 0.25f;

    Vector3 previousPosition = Vector3.zero;

    private void OnDrawGizmosSelected()
    {
        if (transform.position == previousPosition) return;

        float xPos = Mathf.Round((transform.position.x - gridCenter.x) / nodeSize) * nodeSize + gridCenter.x + nodeSize / 2;
        float yPos = Mathf.Round((transform.position.y - gridCenter.y) / nodeSize) * nodeSize + gridCenter.y + nodeSize / 2;
        transform.position = new Vector3(xPos, yPos, 0);
        previousPosition = transform.position;
    }
}

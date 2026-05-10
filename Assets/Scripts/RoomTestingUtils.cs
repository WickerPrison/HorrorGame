using UnityEngine;

public class RoomTestingUtils : MonoBehaviour
{
    RoomSetup _roomSetup;
    RoomSetup roomSetup
    {
        get
        {
            if(_roomSetup == null)
            {
                _roomSetup = GetComponent<RoomSetup>();
            }
            return _roomSetup;
        }
    }

    public Vector2 GetRoomCenter()
    {
        float xCenter = (roomSetup.handles[0].x + roomSetup.handles[1].x) / 2;
        float yCenter = (roomSetup.handles[0].y + roomSetup.handles[2].y) / 2;
        return new Vector2(xCenter, yCenter);
    }

    public void SpawnVisionNode()
    {
        VisionNode visionNode = Instantiate(Resources.Load<GameObject>("Prefabs/VisionNode")).GetComponent<VisionNode>();
        visionNode.transform.position = GetRoomCenter();
    }
}

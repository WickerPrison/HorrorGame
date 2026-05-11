using UnityEngine;

public class EnableChildrenOnRoomDiscovered : MonoBehaviour
{
    [SerializeField] Room room;
    bool showing = false;

    private void Start()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        room.onChangeState += Room_onChangeState;
    }

    private void OnDisable()
    {
        room.onChangeState -= Room_onChangeState;
    }

    private void Room_onChangeState(RoomState state)
    {
        if (!showing && state != RoomState.HIDDEN)
        {
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }
}

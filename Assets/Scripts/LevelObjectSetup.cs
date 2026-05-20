using UnityEngine;

public class LevelObjectSetup : MonoBehaviour
{
#if UNITY_EDITOR
    public void MoveObject(Vector3 direction)
    {
        transform.position += direction;
        if(TryGetComponent<DoorSetup>(out DoorSetup door))
        {
            door.previousPosition = transform.position;
        }
    }
#endif
}

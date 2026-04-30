using UnityEngine;

public class CollisionForwarder : MonoBehaviour
{
    public event System.Action<Collider2D> onTriggerEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger enter");
        onTriggerEnter?.Invoke(collision);
    }
}

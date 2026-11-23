using UnityEngine;

public class SlashAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer slash;
    [SerializeField] float dist;
    [SerializeField] float angle;

    private void Awake()
    {
        slash.enabled = false;
    }

    public void Animation(Vector3 origin, Vector3 direction)
    {
        Vector3 startDirection = Utils.RotateDirection(direction, angle);
        transform.rotation = Utils.Look(startDirection);
        transform.position = origin + startDirection.normalized * dist;

        slash.enabled = true;
    }
}

using UnityEngine;

public class DeadEnemy : MonoBehaviour, IUnhideWhenSeen
{
    [SerializeField] ColorData colorData;
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.color = colorData.deadEnemy;
        sprite.enabled = false;
    }

    public void Unhide()
    {
        sprite.enabled = true;
    }
}

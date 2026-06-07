using UnityEngine;

public class UnhideWhenSeen : MonoBehaviour, IUnhideWhenSeen
{
    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        sprite.enabled = false;
    }

    public void Unhide()
    {
        sprite.enabled = true;
    }
}

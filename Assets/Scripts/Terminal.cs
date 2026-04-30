using UnityEngine;

public class Terminal : MonoBehaviour, IUnhideWhenSeen
{
    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();        
    }

    public void Unhide()
    {
        sprite.enabled = true;
    }
}

using UnityEngine;

public class Terminal : MonoBehaviour, IUnhideWhenSeen
{
    SpriteRenderer[] sprites;
    Room room;

    void Start()
    {
        room = Utils.GetRoom(transform.position);
        room.terminals.Add(this);
        sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.enabled = false;
        }
    }

    public void Unhide()
    {
        foreach(SpriteRenderer sprite in sprites)
        {
            sprite.enabled = true;
        }
    }
}

using UnityEngine;

public enum AltarState
{
    DISABLED, DESECRATED, SANCTIFIED
}

public class Altar : MonoBehaviour, IUnhideWhenSeen
{
    public AltarState altarState;
    [SerializeField] GameObject spritesHolder;
    [SerializeField] ColorData colorData;
    SpriteRenderer[] sprites;
    [System.NonSerialized] public Room room;


    void Start()
    {
        room = Utils.GetRoom(transform.position);
        sprites = spritesHolder.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.enabled = false;
        }
        SetVisuals();
    }

    private void Update()
    {
        room.GainHellfire(-3);
    }

    void SetVisuals()
    {
        foreach(SpriteRenderer sprite in sprites)
        {
            switch(altarState)
            {
                case AltarState.DISABLED:
                    sprite.color = colorData.unpowered;
                    break;
                case AltarState.DESECRATED:
                    sprite.color = colorData.danger;
                    break;
            }
        }
    }

    public void Unhide()
    {
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.enabled = true;
        }
    }
}

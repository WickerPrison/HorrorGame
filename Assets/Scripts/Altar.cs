using System.Linq;
using UnityEngine;

public enum AltarState
{
    DISABLED, DESECRATED, SANCTIFIED
}

public class Altar : MonoBehaviour, IUnhideWhenSeen, IInterceptRightClick
{
    public AltarState altarState;
    [SerializeField] GameObject spritesHolder;
    [SerializeField] ColorData colorData;
    SpriteRenderer[] sprites;
    [System.NonSerialized] public Room room;
    PlayerManager playerManager;


    void Start()
    {
        room = Utils.GetRoom(transform.position);
        playerManager = PlayerManager.i;
        sprites = spritesHolder.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.enabled = false;
        }
        SetVisuals();
    }

    private void Update()
    {
        switch (altarState)
        {
            case AltarState.DESECRATED:
                room.GainHellfire(-3);
                break;
            case AltarState.SANCTIFIED:
                room.GainHolyAura(3);
                break;
        }
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
                case AltarState.SANCTIFIED:
                    sprite.color = colorData.holy;
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

    public bool RightClick()
    {
        return altarState switch
        {
            AltarState.DESECRATED => SanctifyAltar(),
            AltarState.SANCTIFIED => DesecrateAltar(),
            _ => true
        };
    }

    bool SanctifyAltar()
    {
        if (playerManager.selectedUnits.Count != 1 || !playerManager.selectedUnits[0].data.abilities.Contains(Ability.SANCTIFY))
        {
            return true;
        }

        playerManager.selectedUnits[0].unitAbilities.Sanctify(this);
        return false;
    }

    bool DesecrateAltar()
    {
        if (playerManager.selectedUnits.Count != 1 || !playerManager.selectedUnits[0].data.abilities.Contains(Ability.DESECRATE))
        {
            return true;
        }

        playerManager.selectedUnits[0].unitAbilities.Desecrate(this);
        return false;
    }

    public void Sanctify()
    {
        altarState = AltarState.SANCTIFIED;
        SetVisuals();
    }

    public void Desecrate()
    {
        altarState = AltarState.DESECRATED;
        SetVisuals();
    }
}

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
    Vector3[] interactPoints;
    float interactDist = 0.7f;


    void Start()
    {
        room = Utils.GetRoom(transform.position);
        room.altar = this;
        playerManager = PlayerManager.i;
        sprites = spritesHolder.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.enabled = false;
        }
        SetVisuals();
        interactPoints = new Vector3[]
        {
            transform.position + new Vector3(interactDist, 0, 0),
            transform.position + new Vector3(-interactDist, 0, 0),
            transform.position + new Vector3(0, interactDist, 0),
            transform.position + new Vector3(0, -interactDist, 0),
        };
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

    public Vector3 GetDestinationPoint(Vector3 startPos)
    {
        Vector3 closestPoint = Vector3.zero;
        float closestDistance = 1000;
        foreach(Vector3 point in interactPoints)
        {
            float distance = Vector3.Distance(startPos, point);
            if(distance < closestDistance)
            {
                closestPoint = point;
                closestDistance = distance;
            }
        }
        return closestPoint;
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

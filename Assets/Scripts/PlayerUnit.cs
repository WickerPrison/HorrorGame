using Pathfinding;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUnit : MonoBehaviour
{
    private Seeker seeker;
    private AIPath aiPath;
    bool selected = false;
    [SerializeField] SpriteRenderer outline;
    public float visionRange;
    SpriteMask visionMask;
    Terminal interactTerminal = null;
    bool goingToTerminal = false;
    UnitAbilities unitAbilities;
    Action destinationCallback;
    bool atDestination = false;
    [SerializeField] Ability[] abilities;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        unitAbilities = GetComponent<UnitAbilities>();
        PlayerEvents.i.UnitExists(this);
        visionMask = GetComponentInChildren<SpriteMask>();
        visionMask.transform.localScale = visionRange * 2 * Vector3.one;
    }

    private void Update()
    {
        if (!atDestination && aiPath.reachedDestination)
        {
            atDestination = true;
            if(destinationCallback != null)
            {
                destinationCallback();
                destinationCallback = null;
            }
        }


        if(interactTerminal != null && goingToTerminal)
        {
            if (Vector2.Distance(transform.position, interactTerminal.transform.position) <= 1f)
            {
                goingToTerminal = false;
                interactTerminal.StartPowering();
            }
        }
    }

    public void SetSelected(bool isSelected)
    {
        selected = isSelected;
        if (selected)
        {
            outline.color = Color.blue;
        }
        else
        {
            outline.color = Color.white;
        }
    }

    public void PerformAbility(int abilityIndex)
    {
        if (abilities[abilityIndex] == Ability.NONE) return;
        unitAbilities.PerformAbility(abilities[abilityIndex]);
    }

    public void SetDestination(Vector3 destination, Action callback = null)
    {
        aiPath.isStopped = false;
        unitAbilities.InterruptAbilities(); //TODO: consider moving this somewhere else
        destinationCallback = callback;
        atDestination = false;
        seeker.StartPath(transform.position, destination);
        aiPath.destination = destination;
    }

    public void Stop()
    {
        seeker.CancelCurrentPathRequest();
        aiPath.SetPath(null);
        aiPath.isStopped = true;
    }

    public void Death()
    {
        PlayerEvents.i.UnitDeath(this);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        PlayerEvents.i.onDeselectAll += PlayerEvents_onDeselectAll;
    }

    private void OnDisable()
    {
        PlayerEvents.i.onDeselectAll -= PlayerEvents_onDeselectAll;
    }

    private void PlayerEvents_onDeselectAll(object sender, System.EventArgs e)
    {
        SetSelected(false);
    }
}

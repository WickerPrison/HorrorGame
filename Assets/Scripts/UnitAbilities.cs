using Pathfinding;
using UnityEngine;

public enum Ability
{
    NONE, COLLECT, SCAN, POWER, MINE
}

public class UnitAbilities : MonoBehaviour
{
    Room scanningFromRoom = null;
    PlayerUnit playerUnit;
    Terminal poweringTerminal = null;

    private void Start()
    {
        playerUnit = GetComponent<PlayerUnit>();
    }

    private void Update()
    {
        if (scanningFromRoom != null)
        {
            scanningFromRoom.ScanAdjacentRooms(playerUnit);
        }
    }

    public void PerformAbility(Ability abilityType)
    {
        switch (abilityType)
        {
            case Ability.SCAN:
                Scan();
                break;
            case Ability.COLLECT:
                Collect();
                break;
            case Ability.POWER:
                Power();
                break;
        }
    }

    public void InterruptAbilities()
    {
        StopScanning();
        StopPowering();
    }

    void Scan()
    {
        InterruptAbilities();
        playerUnit.Stop();
        scanningFromRoom = Utils.GetRoom(transform.position);
    }

    void StopScanning()
    {
        scanningFromRoom = null;
        GlobalEvents.i.UnitStopScanning(playerUnit);
    }

    public void Collect()
    {
        InterruptAbilities();
        Room room = Utils.GetRoom(transform.position, 0.1f);
        Resource closestResource = null;
        float closestDistance = 1000f;
        foreach (Resource resource in room.resources)
        {
            float currentDist = Vector3.Distance(transform.position, resource.transform.position);
            if (currentDist < closestDistance)
            {
                closestDistance = currentDist;
                closestResource = resource;
            }
        }
        if (closestResource != null)
        {
            playerUnit.SetDestination(closestResource.transform.position, () => closestResource.GetCollected());
        }

    }

    void Power()
    {
        Room room = Utils.GetRoom(transform.position);
        if (room.terminal != null)
        {
            InterruptAbilities();
            playerUnit.SetDestination(room.terminal.GetInteractPoint(), room.terminal.StartPowering);
            poweringTerminal = room.terminal;
        }
    }

    void StopPowering()
    {
        if(poweringTerminal != null)
        {
            poweringTerminal.EndPowering();
            poweringTerminal = null;
        }
    }
}

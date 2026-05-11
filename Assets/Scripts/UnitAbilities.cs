using Pathfinding;
using UnityEngine;
using System.Linq;

public class UnitAbilities : MonoBehaviour
{
    Room scanningFromRoom = null;
    PlayerUnit playerUnit;
    Portal activatingPortal = null;
    Terminal poweringTerminal = null;
    [SerializeField] GameObject minePrefab;

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
            case Ability.MINE:
                PlaceMine();
                break;
        }
    }

    public void InterruptAbilities()
    {
        StopScanning();
        StopPowering();
        StopPortalActivation();
    }

    public void InteractWithPortal()
    {
        if (PortalManager.i.activator != null) return;

        Room room = Utils.GetRoom(transform.position);
        if (room.portal != null)
        {
            InteractWithPortal(room.portal);
        }
    }

    public void InteractWithPortal(Portal portal)
    {
        InterruptAbilities();
        playerUnit.SetDestination(portal.transform.position, () => portal.Activate(playerUnit));
        activatingPortal = portal;
    }

    void StopPortalActivation()
    {
        if(activatingPortal != null)
        {
            activatingPortal.Deactivate();
            activatingPortal = null;
        }
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
            Collect(closestResource);
        }

    }

    public void Collect(Resource resource)
    {
        playerUnit.SetDestination(resource.transform.position, () => resource.GetCollected());
    }

    void Power()
    {
        Room room = Utils.GetRoom(transform.position);
        if (room.terminal != null)
        {
            Power(room.terminal);
        }
    }

    public void Power(Terminal terminal)
    {
        InterruptAbilities();
        playerUnit.SetDestination(terminal.GetInteractPoint(), terminal.StartPowering);
        poweringTerminal = terminal;
    }

    void StopPowering()
    {
        if(poweringTerminal != null)
        {
            poweringTerminal.EndPowering();
            poweringTerminal = null;
        }
    }

    void PlaceMine()
    {
        if(playerUnit.data.mineUses > 0)
        {
            playerUnit.data.mineUses--;
            Instantiate(minePrefab).transform.position = transform.position;
            PlayerEvents.i.UnitStatChange(playerUnit);
        }
    }
}

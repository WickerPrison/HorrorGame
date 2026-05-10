using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Portal : MonoBehaviour, IPowerRooms, IInterceptRightClick
{
    [SerializeField] ColorData colorData;
    [SerializeField] SpriteRenderer pentagram;
    PlayerManager playerManager;

    private void Start()
    {
        Room room = Utils.GetRoom(transform.position);
        room.portal = this;
        room.AddPower(this);

        PortalManager.i.portals.Add(this);

        playerManager = PlayerEvents.i.GetComponent<PlayerManager>();
    }

    public void SetVisualsActive(bool active)
    {
        if (active)
        {
            pentagram.color = colorData.danger;
        }
        else
        {
            pentagram.color = colorData.powered;
        }
    }

    public void Activate(PlayerUnit playerUnit)
    {
        PortalManager.i.ActivatePortal(this, playerUnit);
    }

    public void Deactivate()
    {
        PortalManager.i.DeactivatePortal();
    }

    public bool RightClick()
    {
        if (playerManager.selectedUnits.Count != 1) return true;
        if (PortalManager.i.activator != null && playerManager.selectedUnits[0] != PortalManager.i.activator) return true;

        if(PortalManager.i.activePortal == null || PortalManager.i.activePortal == this)
        {
            playerManager.selectedUnits[0].unitAbilities.InteractWithPortal(this);
        }
        else
        {
            Teleport(this, PortalManager.i.activePortal);
        }

        return false;
    }

    List<IGetTeleported> GetTeleportees()
    {
        return Physics2D.OverlapCircleAll(transform.position, 1)
            .Select(c => c.GetComponent<IGetTeleported>())
            .Where(teleportee => teleportee != null)
            .ToList();
    }

    void Teleport(Portal portal1, Portal portal2)
    {
        List<IGetTeleported> teleportees1 = portal1.GetTeleportees();
        List<IGetTeleported> teleportees2 = portal2.GetTeleportees();
        foreach(IGetTeleported teleportee in teleportees1)
        {
            Vector2 diff = teleportee.transform.position - portal1.transform.position;
            teleportee.transform.position = (Vector2)portal2.transform.position + diff;
            teleportee.GotTeleported();
        }
        foreach (IGetTeleported teleportee in teleportees2)
        {
            Vector2 diff = teleportee.transform.position - portal2.transform.position;
            teleportee.transform.position = (Vector2)portal1.transform.position + diff;
            teleportee.GotTeleported();
        }
        PortalManager.i.DeactivatePortal();
    }
}

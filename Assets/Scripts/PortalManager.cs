using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public static PortalManager i;

    public List<Portal> portals = new List<Portal>();
    public Portal activePortal = null;
    public PlayerUnit activator = null;

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;
    }

    public void ActivatePortal(Portal activatePortal, PlayerUnit playerUnit)
    {
        activator = playerUnit;
        activePortal = activatePortal;
        foreach(Portal portal in portals)
        {
            portal.SetVisualsActive(true);
        }
    }

    public void DeactivatePortal()
    {
        activator = null;
        activePortal = null;
        foreach(Portal portal in portals)
        {
            portal.SetVisualsActive(false);
        }
    }
}

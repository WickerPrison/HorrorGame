using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
    public static PortalManager i;

    [System.NonSerialized] public List<Portal> portals = new List<Portal>();
    [System.NonSerialized] public Portal activePortal = null;
    [System.NonSerialized] public PlayerUnit activator = null;

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
        GlobalEvents.i.PortalActivation(true);
    }

    public void DeactivatePortal()
    {
        activator = null;
        activePortal = null;
        foreach(Portal portal in portals)
        {
            portal.SetVisualsActive(false);
        }
        GlobalEvents.i.PortalActivation(false);
    }

    public void LeaveMission()
    {
        if (activePortal == null) return;
        foreach(Portal portal in portals)
        {
            portal.LeaveMission();
        }
        DeactivatePortal();
    }

    private void OnEnable()
    {
        InputManager.i.onLeaveMission += LeaveMission;
    }

    private void OnDisable()
    {
        InputManager.i.onLeaveMission -= LeaveMission;
    }
}

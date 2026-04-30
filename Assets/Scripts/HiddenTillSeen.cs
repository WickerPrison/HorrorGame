using UnityEngine;

[RequireComponent(typeof(IUnhideWhenSeen))]
public class HiddenTillSeen : MonoBehaviour
{
    bool hidden = true;
    PlayerManager playerManager;
    IUnhideWhenSeen unhideWhenSeen;
    LayerMask layerMask;

    private void Start()
    {
        playerManager = PlayerEvents.i.GetComponent<PlayerManager>();
        unhideWhenSeen = GetComponent<IUnhideWhenSeen>();
        layerMask = LayerMask.GetMask("Default", "Obstacle", "Player");
    }

    void Update()
    {
        if (!hidden) return;
        foreach(PlayerUnit unit in playerManager.allUnits)
        {
            if(Vector3.Distance(transform.position, unit.transform.position) <= unit.visionRange)
            {
                Vector3 direction = unit.transform.position - transform.position;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, unit.visionRange, layerMask);
                if (hit.transform != null && hit.transform.GetComponent<PlayerUnit>())
                {
                    unhideWhenSeen.Unhide();
                    hidden = false;
                    return;
                }
            }
        }
    }
}

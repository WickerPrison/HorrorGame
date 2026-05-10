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

        if (VisionManager.i.FindIsVisible(transform.position))
        {
            unhideWhenSeen.Unhide();
            hidden = false;
        }
    }
}

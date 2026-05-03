using Pathfinding;
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
    float collectRange = 0.3f;
    Resource collectResource = null;
    Room scanningFromRoom = null;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        PlayerEvents.i.UnitExists(this);
        visionMask = GetComponentInChildren<SpriteMask>();
        visionMask.transform.localScale = visionRange * 2 * Vector3.one;
    }

    private void Update()
    {
        if(collectResource != null)
        {
            if(Vector2.Distance(transform.position, collectResource.transform.position) <= collectRange)
            {
                collectResource.GetCollected();
                collectResource = null;
            }
        }

        if (scanningFromRoom != null)
        {
            scanningFromRoom.ScanAdjacentRooms(this);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        seeker.StartPath(transform.position, destination);
        aiPath.destination = destination;
        StopScanning();
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

    public void Scan()
    {
        scanningFromRoom = Utils.GetRoom(transform.position);
    }

    void StopScanning()
    {
        scanningFromRoom = null;
        GlobalEvents.i.UnitStopScanning(this);
    }

    public void Collect()
    {
        Room room = Utils.GetRooms(transform.position, 0.1f)[0];
        Resource closestResource = null;
        float closestDistance = 1000f;
        foreach(Resource resource in room.resources)
        {
            float currentDist = Vector3.Distance(transform.position, resource.transform.position);
            if (currentDist < closestDistance)
            {
                closestDistance = currentDist;
                closestResource = resource;
            }
        }
        if(closestResource != null)
        {
            collectResource = closestResource;
            seeker.StartPath(transform.position, closestResource.transform.position);
            aiPath.destination = closestResource.transform.position;
        }

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

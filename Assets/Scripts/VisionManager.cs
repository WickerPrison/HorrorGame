using System.Collections.Generic;
using UnityEngine;

public class VisionManager : MonoBehaviour
{
    public static VisionManager i;

    LayerMask defaultLayerMask;
    List<IHaveVision> visionList = new List<IHaveVision>();

    private void Awake()
    {
        if (i != null && i != this)
        {
            Destroy(gameObject);
            return;
        }
        i = this;
    }

    private void Start()
    {
        defaultLayerMask = LayerMask.GetMask("Default", "Obstacle", "Player");
    }

    public void AddVision(IHaveVision vision)
    {
        visionList.Add(vision);
    }

    public void RemoveVision(IHaveVision vision)
    {
        visionList.Remove(vision);
    }

    public bool FindIsVisible(Vector3 position)
    {
        foreach (IHaveVision vision in visionList)
        {
            float distance = Vector2.Distance(transform.position, vision.transform.position);

            if(distance < vision.visionRange)
            {
                Vector3 direction = vision.transform.position - position;
                RaycastHit2D hit = Physics2D.Raycast(position, direction.normalized, vision.visionRange, defaultLayerMask);
                //Debug.DrawRay(position, direction.normalized * Vector2.Distance(hit.centroid, position), Color.red, 100);
                if(hit.transform != null && hit.transform.TryGetComponent(out IHaveVision sighted))
                {
                    return true;
                }
            }
        }
        return false;
    }
}

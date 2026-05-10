using UnityEngine;

public interface IHaveVision
{
    public float visionRange { get; set; }
    public void AddToVisionManager();
    public void RemoveFromVisionManager();
    public Transform transform { get; }
}

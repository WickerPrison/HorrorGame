using UnityEngine;

public interface IGetTeleported
{
    public Transform transform { get; }
    public void GotTeleported();
    public void LeaveMission();
}

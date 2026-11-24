using UnityEngine;

public interface IFightDemons: ITakeDamage
{
    void RegisterWithMissionManager();
    void RemoveFromMissionManager();
}

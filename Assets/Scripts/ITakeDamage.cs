using UnityEngine;

public interface ITakeDamage
{
    Transform transform { get; }
    public void TakeDamage(int amount);
}

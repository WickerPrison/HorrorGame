using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "Scriptable Objects/ColorData")]
public class ColorData : ScriptableObject
{
    public Color powered;
    public Color unpowered;
    public Color safe;
    public Color danger;
    public Color unscannable;
}

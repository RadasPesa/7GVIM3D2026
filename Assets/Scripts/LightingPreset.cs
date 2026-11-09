using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "Scriptables/Lighting Preset")]
public class LightingPreset : ScriptableObject
{
    public Gradient ambientColor;
    public Gradient directionalColor;
}

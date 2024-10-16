using UnityEngine;

namespace GameCore.LightCycle
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Lighting Preset", menuName = "Scriptables/Lighting Preset", order = 0)]
    public class LightingPreset : ScriptableObject
    {
         public Gradient ambientColor;
        public Gradient directionalColor;
        public Gradient fogColor;
    }
}
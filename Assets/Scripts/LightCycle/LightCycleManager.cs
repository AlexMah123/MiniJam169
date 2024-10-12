﻿using UnityEngine;
using UnityEngine.Serialization;

namespace LightCycle
{
    [ExecuteAlways]
    public class LightCycleManager : MonoBehaviour
    {
        //Scene References
        [Header("Light Configs")]
        [SerializeField] private Light directionalLight;
        [SerializeField] private LightingPreset preset;

        [Header("CurrentTime of Day - In hours")] 
        [Range(0, 24)] public float timeOfDay;

        [Header("Hour per real time seconds")]
        [SerializeField] private int realTimeSeconds = 10;
        
        private void Update()
        {
            if (preset == null)
            {
                Debug.LogWarning("No lighting preset set");
                return;
            }

            if (Application.isPlaying)
            {
                //(Replace with a reference to the game time)
                timeOfDay += Time.deltaTime / realTimeSeconds;
                timeOfDay %= 24; //Modulus to ensure always between 0-24
                UpdateLighting(timeOfDay / 24f);
            }
            else
            {
                UpdateLighting(timeOfDay / 24f);
            }
        }
        
        private void UpdateLighting(float timePercent)
        {
            //Set ambient and fog
            RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
            RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);

            //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
            if (directionalLight != null)
            {
                directionalLight.color = preset.directionalColor.Evaluate(timePercent);

                directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
            }
        }

        //Try to find a directional light to use if we haven't set one
        private void OnValidate()
        {
            if (directionalLight != null)
                return;

            //Search for lighting tab sun
            if (RenderSettings.sun != null)
            {
                directionalLight = RenderSettings.sun;
            }
            else
            {
                //Search scene for light that fits criteria (directional)
                Light[] lights = GameObject.FindObjectsOfType<Light>();
                foreach (Light light in lights)
                {
                    if (light.type == LightType.Directional)
                    {
                        directionalLight = light;
                        return;
                    }
                }
            }
        }
    }
}
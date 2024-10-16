using System.Collections;
using UnityEngine;

namespace GameCore.LightCycle
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
        [SerializeField] private float realTimeSeconds = 10f;
        
        private void Update()
        {
            if (preset == null)
            {
                Debug.LogWarning("No lighting preset set");
                return;
            }

            if (!Application.isPlaying)
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
        
        public IEnumerator StartTimelapse(int timeRequired)
        {
            float elapsedTime = 0;

            var timeRequiredInRealSeconds = timeRequired * realTimeSeconds;
            
            while (elapsedTime < timeRequiredInRealSeconds)
            {
                //every hour = realTimeSeconds
                timeOfDay += Time.deltaTime / realTimeSeconds;
                timeOfDay %= 24; //Modulus to ensure always between 0-24
                UpdateLighting(timeOfDay / 24f);
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            timeOfDay %= 24;
            UpdateLighting(timeOfDay / 24f);
        }
        
        public IEnumerator TimelapseToTime(int timeToTimelapseTill)
        {
            float elapsedTime = 0;

            float targetTime = timeToTimelapseTill % 24;
            
            // If the target time is less than the current time, it's a new day
            if (targetTime <= timeOfDay)
            {
                targetTime += 24;
            }
            
            float startTime = timeOfDay;
            float timeRequiredInRealSeconds = Mathf.Abs(targetTime - startTime) * realTimeSeconds;
            
            while (elapsedTime < timeRequiredInRealSeconds)
            {
                //every hour = realTimeSeconds
                timeOfDay += Time.deltaTime / realTimeSeconds;
                timeOfDay %= 24; //Modulus to ensure always between 0-24
                UpdateLighting(timeOfDay / 24f);
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            timeOfDay = targetTime % 24;
            UpdateLighting(timeOfDay / 24f);
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
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace LightCycle.WorldLights
{
    [ExecuteAlways]
    public class WorldLightsController : MonoBehaviour
    {
        [Header("Lights Collection")]
        public List<GameObject> lightsContainer = new();
        
        [Header("Lights Config")]
        [SerializeField] LightCycleManager _lightCycleManager;
        public float turnOnHour = 18f;
        public float turnOffHour = 6f;
        
        private List<Light> _lights = new ();

        private void Start()
        {
            PopulateLights();
        }

        private void Update()
        {
            if (_lightCycleManager == null)
            {
                Debug.LogWarning("LightCycleManager is not set in WorldLightsController");
                return;
            }
            
            if (lightsContainer.Count == 0)
            {
                Debug.LogWarning("Lights collection is empty in WorldLightsController");
                return;
            }
            
            if (_lightCycleManager.timeOfDay  >= turnOnHour || _lightCycleManager.timeOfDay  < turnOffHour)
            {
                SetLights(true);
            }
            else
            {
                SetLights(false);
            }
        }

        private void SetLights(bool state)
        {
            foreach (var light in _lights.Where(light => light.enabled != state))
            {
                light.enabled = state;
            }
        }

        private void OnValidate()
        {
            PopulateLights();
        }

        private void PopulateLights()
        {
            _lights.Clear();

            if (lightsContainer.Count == 0) return;
            
            foreach (var container in lightsContainer)
            {
                if(container == null) continue;
                
                _lights.AddRange(container.GetComponentsInChildren<Light>());
            }
        }
    }
}
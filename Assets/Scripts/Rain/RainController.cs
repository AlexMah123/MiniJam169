using System;
using Audio;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rain
{
    public class RainController : MonoBehaviour
    {
        [Header("Rain Config")]
        [SerializeField] private ParticleSystem rain;
        [SerializeField] private float minDurationSecond = 30f;
        [SerializeField] private float maxDurationSecond = 120f;
        [SerializeField] private bool isStartWithRain = true;

        private float _currentTimer;

        private void Awake()
        {
            if (rain == null)
            {
                Debug.LogError("Rain ParticleSystem is null");
            }

            if (!isStartWithRain)
            {
                rain.Stop();
            }
        }

        private void Start()
        {
            SetRandomTimer();
        }

        private void Update()
        {
            _currentTimer -= Time.deltaTime;

            if (_currentTimer <= 0)
            {
                ToggleRain();
                SetRandomTimer();
            }
        }

        private void SetRandomTimer()
        {
            _currentTimer = Random.Range(minDurationSecond, maxDurationSecond);
        }

        private void ToggleRain()
        {
            if(rain == null) return;

            SFXManager.Instance.PlaySoundFXClip("CityAmbience", this.transform);
            
            if (rain.isPlaying)
            {
                rain.Stop();
            }
            else
            {
                rain.Play();
            }
        }
    }
}
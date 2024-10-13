using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class SoundMixerManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        private const string MasterVolumeKey = "MasterVolume";
        private const string MusicVolumeKey = "MusicVolume";
        private const string SfxVolumeKey = "SFXVolume";

        private void Awake()
        {
            if (masterVolumeSlider == null || musicVolumeSlider == null || sfxVolumeSlider == null)
            {
                throw new MissingReferenceException("One or more sliders are missing reference. Check slider reference");
            }
        }

        private void Start()
        {
            LoadVolumeSettings();
        }

        public void SetMasterVolume(float level)
        {
            audioMixer.SetFloat(MasterVolumeKey, Mathf.Log10(level) * 20f);
            PlayerPrefs.SetFloat(MasterVolumeKey, level);
        }

        public void SetMusicVolume(float level)
        {
            audioMixer.SetFloat(MusicVolumeKey, Mathf.Log10(level) * 20f);
            PlayerPrefs.SetFloat(MusicVolumeKey, level);
        }

        public void SetSfxVolume(float level)
        {
            audioMixer.SetFloat(SfxVolumeKey, Mathf.Log10(level) * 20f);
            PlayerPrefs.SetFloat(SfxVolumeKey, level);
        }

        private void LoadVolumeSettings()
        {
            float defaultVolume = 0.5f; // Default volume
            float masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, defaultVolume);
            float musicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, defaultVolume);
            float sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, defaultVolume);

            masterVolumeSlider.value = masterVolume;
            musicVolumeSlider.value = musicVolume;
            sfxVolumeSlider.value = sfxVolume;

            SetMasterVolume(masterVolume);
            SetMusicVolume(musicVolume);
            SetSfxVolume(sfxVolume);
        }
    }
}
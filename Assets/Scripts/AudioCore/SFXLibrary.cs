using System;
using System.Linq;
using UnityEngine;

namespace AudioCore
{
    [Serializable]
    public struct SoundEffect
    {
        public string groupID;
        public AudioClip[] clips;
    }
    
    public class SFXLibrary : MonoBehaviour
    {
        public SoundEffect[] soundEffects;

        public AudioClip GetAudioClip(string name)
        {
            var audioClip = soundEffects.FirstOrDefault(sfx => sfx.groupID == name);

            if (audioClip.Equals(default(SoundEffect)))
            {
                throw new NullReferenceException($"{name} clip was not found");
            }

            return audioClip.clips[UnityEngine.Random.Range(0, audioClip.clips.Length)];
        }
    }
}
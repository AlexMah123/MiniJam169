using System.Collections;
using AudioCore;
using UnityEngine;
using GameCore.SceneTransition;

namespace UserInterface.Button
{
    public class SceneTransitionHandler : MonoBehaviour
    {
        public SceneType sceneToTransition;
        private Coroutine coroutine;

        public void LoadScene()
        {
            if (coroutine != null) return;

            var audioClip = SFXManager.Instance.sfxLibrary.GetAudioClip("ButtonClick");
            SFXManager.Instance.PlaySoundFXClip(audioClip, transform);

            coroutine = StartCoroutine(HandleLoadScene(audioClip.length));
        }

        private IEnumerator HandleLoadScene(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);

            SceneTransitionManager.Instance.LoadScene(sceneToTransition);
            coroutine = null;
        }
    }
}
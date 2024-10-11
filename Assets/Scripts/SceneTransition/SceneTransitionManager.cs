using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Audio;

namespace SceneTransition
{
    [Serializable]
    public enum SceneType
    {
        Exit = -1,
        Menu,
        Game,
    }
    
    public class SceneTransitionManager : MonoBehaviour
    {
        public static SceneTransitionManager Instance;
        private bool isTransitioning = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void LoadScene(SceneType scene)
        {
            //early return to prevent spamming
            if (isTransitioning) return;

            SFXManager.Instance.PlaySoundFXClip("SceneTransition", transform);
            StartCoroutine(LoadSceneAsync(scene));
        }

        private IEnumerator LoadSceneAsync(SceneType sceneType)
        {
            //set flag to true
            isTransitioning = true;


            if (sceneType == SceneType.Exit)
            {
                Application.Quit();

                //#DEBUG
                Debug.Log("Quitting Game");

                //early reset
                isTransitioning = false;
                yield break;
            }

            //load scene
            AsyncOperation scene = SceneManager.LoadSceneAsync((int)sceneType);
            scene.allowSceneActivation = false;

            //play animation to transition into new scene
            yield return new WaitForSeconds(0.1f);

            scene.allowSceneActivation = true;

            //reset flag
            isTransitioning = false;
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.Tutorial
{
    public class TutorialController : MonoBehaviour
    {
        [Header("Tutorial Configs")]
        [SerializeField] private List<Sprite> tutorialSprites;
        [SerializeField] private Image tutorialImage;
        
        [Header("Buttons")]
        [SerializeField] private UnityEngine.UI.Button nextButton;
        [SerializeField] private UnityEngine.UI.Button prevButton;
        [SerializeField] private UnityEngine.UI.Button closeButton;
        
        private int _currentTutorialIndex;

        private void Awake()
        {
            if (tutorialSprites.Count == 0)
            {
                Debug.LogError("No tutorial sprites found");
                return;
            }
        }

        private void Start()
        {
            _currentTutorialIndex = 0;
            ChangeImage(0);
        }

        public void NextGuideButton()
        {
            ChangeImage(1);
        }

        public void PrevGuideButton()
        {
            ChangeImage(-1);
        }

        private void ChangeImage(int direction)
        {
            _currentTutorialIndex = Mathf.Clamp(_currentTutorialIndex + direction, 0, tutorialSprites.Count - 1);
            
            //update image
            if (_currentTutorialIndex >= 0 && _currentTutorialIndex < tutorialSprites.Count)
            {
                tutorialImage.sprite = tutorialSprites[_currentTutorialIndex];
            }
            
            UpdateButtonState();
        }
        
        void UpdateButtonState()
        {
            // enable after first image
            prevButton.gameObject.SetActive(_currentTutorialIndex > 0);

            // enable before last image
            nextButton.gameObject.SetActive(_currentTutorialIndex < tutorialSprites.Count - 1);
            
            //only enable if you are on the last image
            closeButton.gameObject.SetActive(_currentTutorialIndex == tutorialSprites.Count - 1);
        }
        
    }
}
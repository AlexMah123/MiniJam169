using System;
using Decision;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.Decision
{
    public class DecisionUI : MonoBehaviour
    {
        [Header("Decision Config")]
        [SerializeField] private DecisionSO decision;

        [Header("UI Element")]
        [SerializeField] private Image iconImage;
        
        private Camera _mainCamera;

        //event declaration
        public event Action<DecisionSO> OnDecisionInteract;
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            iconImage.sprite = decision.icon;
        }

        private void LateUpdate()
        {
            //look at the camera
            transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward, _mainCamera.transform.rotation * Vector3.up);
        }
        
        public void Interact()
        {
            //rect transform
            OnDecisionInteract?.Invoke(decision);
        }
    }
}
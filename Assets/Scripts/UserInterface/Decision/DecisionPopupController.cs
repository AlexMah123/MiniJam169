using System;
using Decision;
using TMPro;
using UnityEngine;

namespace UserInterface.Decision
{
    public class DecisionPopupController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject popupUI;

        [SerializeField] private TextMeshProUGUI decisionName;
        [SerializeField] private TextMeshProUGUI decisionDescription;
        [SerializeField] private TextMeshProUGUI decisionEffect;

        [SerializeField] private TextMeshProUGUI timeRequired;
        [SerializeField] private TextMeshProUGUI policeAlert;
        [SerializeField] private TextMeshProUGUI mobsterAlert;
        [SerializeField] private TextMeshProUGUI mobsterCaught;
        
        private Camera _mainCamera;
        
        private void OnEnable()
        {
            DecisionUI[] uiCollection = FindObjectsOfType<DecisionUI>();

            foreach (var ui in uiCollection)
            {
                ui.OnDecisionInteract += HandleDecisionInteract;
            }
        }
        
        private void OnDisable()
        {
            DecisionUI[] uiCollection = FindObjectsOfType<DecisionUI>();

            foreach (var ui in uiCollection)
            {
                if(ui == null) continue;
                
                ui.OnDecisionInteract -= HandleDecisionInteract;
            }
        }
        
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            popupUI.gameObject.SetActive(false);
        }
        
        private void HandleDecisionInteract(DecisionSO obj)
        {
            popupUI.gameObject.SetActive(true);
            UpdatePopupUI(obj);
        }

        private void UpdatePopupUI(DecisionSO obj)
        {
            decisionName.text = obj.decisionName;
            decisionDescription.text = $"<color=yellow>Description</color>: {obj.decisionDescription}";
            decisionEffect.text = $"{obj.decisionEffect}";
            
            timeRequired.text = $"Time required to execute: {obj.decisionOutcome.timeRequired} Hours";
            
            policeAlert.text = $"{obj.decisionOutcome.policeAlertRaised}";
            mobsterAlert.text = $"{obj.decisionOutcome.mobsterAlertRaised}";
            mobsterCaught.text = $"{obj.decisionOutcome.mobsterCaught}";
        }
    }
}
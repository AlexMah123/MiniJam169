using DecisionCore;
using TMPro;
using UnityEngine;

using GameCore;

namespace UserInterface.DecisionUI
{
    public class DecisionPopupController : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject popupUI;
        
        [Space(10)]
        [SerializeField] private TextMeshProUGUI decisionName;
        [SerializeField] private TextMeshProUGUI decisionDescription;
        [SerializeField] private TextMeshProUGUI decisionEffect;

        [SerializeField] private TextMeshProUGUI timeRequired;
        [SerializeField] private TextMeshProUGUI policeAlert;
        [SerializeField] private TextMeshProUGUI mobsterAlert;
        [SerializeField] private TextMeshProUGUI mobsterCaught;
        
        private Camera _mainCamera;
        private DecisionSO _decisionSelected;
        private GameObject _decisionUISelected;
        
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

        public void ConfirmDecision()
        {
            GameManager.Instance.MakeChoice(_decisionSelected, _decisionUISelected);
            _decisionUISelected.SetActive(false);
        }

        public void CancelDecision()
        {
            _decisionUISelected = null;
            _decisionSelected = null;
        }
        
        private void HandleDecisionInteract(DecisionSO scriptable, GameObject uiObj)
        {
            if (scriptable == null)
            {
                Debug.LogError("DecisionSO is null when confirming decision");
                return;
            }

            if (uiObj == null)
            {
                Debug.LogError("UI Object is null when confirming decision");
                return;
            }
            
            popupUI.gameObject.SetActive(true);
            UpdatePopupUI(scriptable, uiObj);
        }

        private void UpdatePopupUI(DecisionSO scriptable, GameObject uiObj)
        {
            //cache current obj and scriptable when selected.
            _decisionUISelected = uiObj;
            _decisionSelected = scriptable;

            decisionName.text = scriptable.decisionName;
            decisionDescription.text = $"<color=yellow>Description</color>: {scriptable.decisionDescription}";
            decisionEffect.text = $"{scriptable.decisionEffect}";

            timeRequired.text = $"Time required to execute: <color=magenta>{scriptable.decisionOutcome.timeRequired} Hour</color>";

            policeAlert.text = $"{scriptable.decisionOutcome.policeAlertRaised}";
            mobsterAlert.text = $"{scriptable.decisionOutcome.mobsterAlertRaised}";
            mobsterCaught.text = $"{scriptable.decisionOutcome.mobsterCaught}";
        }
    }
}
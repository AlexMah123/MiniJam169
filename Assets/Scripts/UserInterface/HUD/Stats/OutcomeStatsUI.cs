using System;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UserInterface.HUD.Stats
{
    public class OutcomeStatsUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _policeAlertText;
        [SerializeField] TextMeshProUGUI _mobsterAlertText;
        [SerializeField] TextMeshProUGUI _mobsterCaughtText;
        
        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnOutcomeChanged -= OnOutcomeChanged;
            }
        }
        
        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnOutcomeChanged += OnOutcomeChanged;
            }
        }
        
        private void OnOutcomeChanged(GameOutcome outcome, int maxPoliceAlert, int maxMobsterAlert)
        {
            _policeAlertText.text = $"{outcome.policeAlertRaised}/{maxPoliceAlert}";
            _mobsterAlertText.text = $"{outcome.mobsterAlertRaised}/{maxMobsterAlert}";
            _mobsterCaughtText.text = $"{outcome.mobsterCaught}";
        }
        
    }
}
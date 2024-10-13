using System;
using Game;
using TMPro;
using UnityEngine;

namespace UserInterface.EndCondition
{
    public class EndConditionController : MonoBehaviour
    {
        [Header("Parent")]
        [SerializeField] private GameObject endConditionPanel;
        
        [Header("UI Element")]
        [SerializeField] private TextMeshProUGUI daySurvived;
        [SerializeField] private TextMeshProUGUI mobstersCaught;

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameOver -= ShowEndCondition;
            }
        }

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameOver += ShowEndCondition;
            }
        }
        
        private void ShowEndCondition(GameOutcome obj)
        {
            endConditionPanel.gameObject.SetActive(true);
            
            daySurvived.text = $"Day Survived: {obj.day}";
            mobstersCaught.text = $"Mobsters Caught: {obj.mobsterCaught}";
        }
    }
}
using System;
using Game;
using UnityEngine;

namespace UserInterface.EndCondition
{
    public class EndConditionController : MonoBehaviour
    {
        [SerializeField] private GameObject endConditionPanel;
        
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
        }
    }
}
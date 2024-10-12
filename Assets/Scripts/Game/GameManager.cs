using System;
using System.Collections.Generic;
using Decision;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [Serializable]
    public struct GameOutcome
    {
        public int currentCost;
        public int timeRemaining;
        public int policeAlertRaised;
        public int mobsterAlertRaised;
        public int mobsterCaught;
    }
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("GameConfig")] 
        public int playerStartingCost = 3;
        public int timeHoursPerDay = 18;
        public int maxPoliceAlert = 100;

        [Header("GameState")]
        public GameOutcome currentOutcome;
        
        private int _currentPlayerCost;
        
        //event declaration
        public event Action<GameOutcome> OnGameOver;
        public event Action<GameOutcome> OnOutcomeChanged;
        
        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            
            currentOutcome = new GameOutcome();
        }

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            StartDay();
            OnOutcomeChanged?.Invoke(currentOutcome);
        }

        private void StartDay()
        {
            currentOutcome.timeRemaining = timeHoursPerDay;
            currentOutcome.currentCost = playerStartingCost;
        }
        
        public void MakeChoice(DecisionSO choice)
        {
            //cost
            currentOutcome.currentCost -= choice.decisionOutcome.cost;
            
            //apply choice effect
            currentOutcome.timeRemaining -= choice.decisionOutcome.timeRequired;
            currentOutcome.policeAlertRaised += choice.decisionOutcome.policeAlertRaised;
            currentOutcome.mobsterAlertRaised += choice.decisionOutcome.mobsterAlertRaised;
            currentOutcome.mobsterCaught += choice.decisionOutcome.mobsterCaught;
            
            OnOutcomeChanged?.Invoke(currentOutcome);
            
            if (IsCaught())
            {
                OnGameOver?.Invoke(currentOutcome);
            }
        }

        private bool IsCaught()
        {
            return currentOutcome.policeAlertRaised == maxPoliceAlert;
        }
    }
}
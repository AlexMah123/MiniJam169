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
        public int policeAwarenessRaised;
        public int mobsterAwarenessRaised;
        public int mobsterCaught;
    }
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("GameConfig")] 
        public int playerStartingCost = 3;
        public int timeHoursPerDay = 18;
        
        [Header("GameState")]
        public GameOutcome currentOutcome;
        public GameOutcome endingOutcome;

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

        public void StartGame()
        {
            StartDay();
            OnOutcomeChanged?.Invoke(currentOutcome);
        }
        
        
        public void StartDay()
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
            currentOutcome.policeAwarenessRaised += choice.decisionOutcome.policeAwarenessRaised;
            currentOutcome.mobsterAwarenessRaised += choice.decisionOutcome.mobsterAwarenessRaised;
            currentOutcome.mobsterCaught += choice.decisionOutcome.mobsterCaught;
            
            OnOutcomeChanged?.Invoke(currentOutcome);
            
            if (IsCaught())
            {
                OnGameOver?.Invoke(currentOutcome);
            }
        }

        private bool IsCaught()
        {
            return currentOutcome.policeAwarenessRaised == endingOutcome.policeAwarenessRaised;
        }
    }
}
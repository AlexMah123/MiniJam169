using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Decision;
using UserInterface.Decision;
using Random = UnityEngine.Random;

namespace Game
{
    [Serializable]
    public struct GameOutcome
    {
        public int day;
        public int timeRemaining;
        public int policeAlertRaised;
        public int mobsterAlertRaised;
        public int mobsterCaught;
    }
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [Header("Starting GameConfig")] 
        public int timeHoursPerDay = 18;
        public int maxPoliceAlert = 100;
        public int maxMobsterAlert = 100;
        public GameObject decisionContainer;
        
        [Header("GameConfig")]
        public int maxMobsterSpawn = 10;
        public int maxPoliceSpawn = 10;
        public int policeAlertDrop = 10;
        public int mobsterAlertDrop = 10;
        public float minMobsterMultiplier = 0.25f;
        public float maxMobsterMultiplier = 0.75f;
        
        [Header("GameState")]
        public GameOutcome currentOutcome;


        private List<DecisionUI> _decisionUIList = new();
        
        //event declaration
        public event Action<GameOutcome> OnGameOver;
        public event Action<GameOutcome, int, int> OnOutcomeChanged;
        
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

            if (decisionContainer == null)
            {
                Debug.LogError("No Decision Container found");
                return;
            }
            
            currentOutcome = new GameOutcome();
            _decisionUIList = decisionContainer.GetComponentsInChildren<DecisionUI>().ToList();
        }

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            StartDay();
        }

        private void StartDay()
        {
            currentOutcome.day++;
            currentOutcome.timeRemaining = timeHoursPerDay;

            RandomizeAlertDrop();
            OnOutcomeChanged?.Invoke(currentOutcome, maxPoliceAlert, maxMobsterAlert);
        }
        
        public void MakeChoice(DecisionSO choice)
        {
            //apply choice effect
            currentOutcome.timeRemaining -= choice.decisionOutcome.timeRequired;
            currentOutcome.policeAlertRaised += choice.decisionOutcome.policeAlertRaised;
            currentOutcome.mobsterAlertRaised += choice.decisionOutcome.mobsterAlertRaised;
            currentOutcome.mobsterCaught += choice.decisionOutcome.mobsterCaught;
            
            OnOutcomeChanged?.Invoke(currentOutcome, maxPoliceAlert, maxMobsterAlert);
            
            if (IsCaught())
            {
                OnGameOver?.Invoke(currentOutcome);
            }
        }

        private void RandomizeAlertDrop()
        {
            var randomPoliceAlert = Random.Range(0, policeAlertDrop);
            var randomMobsterAlert = Random.Range(0, mobsterAlertDrop);
            
            currentOutcome.policeAlertRaised -= randomPoliceAlert;
            currentOutcome.mobsterAlertRaised -= randomMobsterAlert;
            
            currentOutcome.policeAlertRaised = Mathf.Clamp(currentOutcome.policeAlertRaised, 0, maxPoliceAlert);
            currentOutcome.mobsterAlertRaised = Mathf.Clamp(currentOutcome.mobsterAlertRaised, 0, maxMobsterAlert);
            
            OnOutcomeChanged?.Invoke(currentOutcome, maxPoliceAlert, maxMobsterAlert);
        }

        #region Helper Methods
        private bool IsCaught()
        {
            return currentOutcome.policeAlertRaised == maxPoliceAlert;
        }

        private int CalculateMobsterSpawn(int currentMobsterAlert)
        {
            return Mathf.Max(0, (int)(maxMobsterSpawn * (1f - (float)currentMobsterAlert /maxMobsterAlert)));
        }

        private int CalculatePoliceSpawn(int currentPoliceAlert)
        {
            return Mathf.Max(0, (int)(maxPoliceSpawn * ((float)currentPoliceAlert / maxPoliceAlert)));
        }

        private int CalculatePassiveMobsterCaught(float remainingTime)
        {
            float randomMultiplier = Random.Range(minMobsterMultiplier, maxMobsterMultiplier);

            int passiveMobsterAmount = Mathf.RoundToInt(remainingTime * randomMultiplier);

            return passiveMobsterAmount;
        }
        #endregion
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using AudioCore;
using DecisionCore;
using GameCore.LightCycle;
using UserInterface.DecisionUI;
using Random = UnityEngine.Random;

namespace GameCore
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
        public int startingTimeHours = 6;
        public int maxDecisionToMake = 3;
        public int timeHoursPerDay = 18;
        public int maxPoliceAlert = 100;
        public int maxMobsterAlert = 100;
        public GameObject decisionContainer;

        [Header("GameConfig")] 
        public int maxMobsterSpawn = 10;
        public int minPoliceSpawn = 2;
        public int maxPoliceSpawn = 10;
        public int policeAlertDrop = 10;
        public int mobsterAlertDrop = 10;
        public float minMobsterMultiplier = 0.25f;
        public float maxMobsterMultiplier = 0.75f;
        
        [Header("Dependencies")]
        [SerializeField] private LightCycleManager lightCycleManager;

        [SerializeField] private GameObject skipActionButton;
        [SerializeField] private GameObject gameActionObj;
        [SerializeField] private TextMeshProUGUI gameActionText;
        
        [Header("GameState")]
        public GameOutcome currentOutcome;
        
        //private
        private List<DecisionUI> _decisionUIList = new();
        private List<Button> _decisionButtons = new();
        
        //runtime
        private int _currentDecisionCount = 0;
        private List<GameObject> _currentDecisionObjMade = new();
        
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

            if (lightCycleManager == null)
            {
                Debug.LogError("No LightCycleManager found");
                return;
            }
            
            currentOutcome = new GameOutcome();
            _decisionUIList = decisionContainer.GetComponentsInChildren<DecisionUI>().ToList();
            _decisionButtons = _decisionUIList.Select(uiObj => uiObj.GetComponent<Button>()).ToList();
        }

        private void Start()
        {
            StartDay();
        }
        
        private void StartDay()
        {
            //reset count, reset everything
            SetUIObjState(true);
            SetUIButtonState(true);
            SetSkipActionObjState(true);
            gameActionObj.SetActive(true);
            
            _currentDecisionCount = maxDecisionToMake;
            gameActionText.text = $"Max <color=red>3 decisions</color> per day. <color=yellow>{_currentDecisionCount} left</color>.. ";
            
            currentOutcome.timeRemaining = timeHoursPerDay;
            currentOutcome.day++;
            
            RandomizeAlertDrop();
            OnOutcomeChanged?.Invoke(currentOutcome, maxPoliceAlert, maxMobsterAlert);
            
            if (IsPlayerCaught())
            {
                OnGameOver?.Invoke(currentOutcome);
                StopAllCoroutines();
            }
        }
        
        public void SkipDecision()
        {
            gameActionText.text = "<color=#A5FFF1>Skipping decisions... Time will be passing...\nYou stop crime normally</color> ";
            
            //call coroutine to make choice and start timelapse
            StartCoroutine(ResetDay());
        }
        
        public void MakeChoice(DecisionSO choice, GameObject uiObj)
        {
            SFXManager.Instance.PlaySoundFXClip("DecisionSFX", uiObj.transform);
            _currentDecisionObjMade.Add(uiObj);
            
            //temporarily disable DecisionUI
            SetUIButtonState(false);
            SetSkipActionObjState(false);

            gameActionText.text = $"<color=#A5FFF1>Preparing for {choice.decisionOutcome.timeRequired} hours.. \n Time is ticking... </color>";
            
            //call coroutine to make choice and start timelapse
            StartCoroutine(DecisionProcess(choice));
        }
        
        private IEnumerator DecisionProcess(DecisionSO choice)
        {
            yield return lightCycleManager.StartCoroutine(lightCycleManager.StartTimelapse(choice.decisionOutcome.timeRequired));
            
            //after timelapse, enable all ui
            SetUIButtonState(true);
            SetUIObjState(true);
            SetSkipActionObjState(true);
            
            SFXManager.Instance.PlaySoundFXClip("CrimeSFX", this.transform);

            //apply choice effect
            currentOutcome.timeRemaining -= choice.decisionOutcome.timeRequired;
            currentOutcome.policeAlertRaised += choice.decisionOutcome.policeAlertRaised;
            currentOutcome.mobsterAlertRaised += choice.decisionOutcome.mobsterAlertRaised;
            currentOutcome.mobsterCaught += choice.decisionOutcome.mobsterCaught;
            OnOutcomeChanged?.Invoke(currentOutcome, maxPoliceAlert, maxMobsterAlert);
            
            //decrement the count
            _currentDecisionCount--;
            
            //update the text
            if (_currentDecisionCount > 0)
            {
                gameActionText.text = $"Max <color=red>3 decisions</color> per day.\n<color=yellow>{_currentDecisionCount} left</color>...";
            }
            else
            {
                //max decisions made, timelapse to start of day
                gameActionText.text = "<color=#A5FFF1>Max decisions made... Time will be passing...\nYou stop crime normally</color>...";
                
                //call coroutine to make choice and start timelapse
                StartCoroutine(ResetDay());
                
                SFXManager.Instance.PlaySoundFXClip("PoliceSFX", this.transform);
            }
            
            if (IsPlayerCaught())
            {
                gameActionObj.SetActive(false);
                StopAllCoroutines();

                OnGameOver?.Invoke(currentOutcome);
            }
        }

        private IEnumerator ResetDay()
        {
            SetUIObjState(false);
            SetSkipActionObjState(false);
            
            yield return lightCycleManager.StartCoroutine(lightCycleManager.TimelapseToTime(startingTimeHours));
            
            currentOutcome.policeAlertRaised += CalculatePoliceSpawn(currentOutcome.policeAlertRaised);
            currentOutcome.mobsterAlertRaised += CalculateMobsterSpawn(currentOutcome.mobsterAlertRaised);
            currentOutcome.mobsterCaught += CalculatePassiveMobsterCaught(currentOutcome.timeRemaining);
            
            currentOutcome.policeAlertRaised = Mathf.Clamp(currentOutcome.policeAlertRaised, 0, maxPoliceAlert);
            currentOutcome.mobsterAlertRaised = Mathf.Clamp(currentOutcome.mobsterAlertRaised, 0, maxMobsterAlert);
            
            OnOutcomeChanged?.Invoke(currentOutcome, maxPoliceAlert, maxMobsterAlert);

            ResetDecision();
            StartDay();
        }

        private void ResetDecision()
        {
            _currentDecisionObjMade.Clear();
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
        private void SetSkipActionObjState(bool state)
        {
            skipActionButton.gameObject.SetActive(state);
        }
        
        private void SetUIButtonState(bool state)
        {
            foreach (var uiButton in _decisionButtons)
            {
                uiButton.interactable = state;
            }
        }
        
        private void SetUIObjState(bool state)
        {
            //loop and check if uiObj is contained in _currentDecisionObjMade
            foreach (var uiObj in _decisionUIList.Where(uiObj => !_currentDecisionObjMade.Contains(uiObj.gameObject)))
            {
                uiObj.gameObject.SetActive(state);
            }
        }
        
        private bool IsPlayerCaught()
        {
            return currentOutcome.policeAlertRaised >= maxPoliceAlert;
        }

        private int CalculateMobsterSpawn(int currentMobsterAlert)
        {
            return Mathf.Max(0, (int)(maxMobsterSpawn * (1f - (float)currentMobsterAlert /maxMobsterAlert)));
        }

        private int CalculatePoliceSpawn(int currentPoliceAlert)
        {
            return Mathf.Max(minPoliceSpawn, (int)(maxPoliceSpawn * ((float)currentPoliceAlert / maxPoliceAlert)));
        }

        private int CalculatePassiveMobsterCaught(float remainingTime)
        {
            float randomMultiplier = Random.Range(minMobsterMultiplier, maxMobsterMultiplier);

            int passiveMobsterAmount = Mathf.RoundToInt(remainingTime * randomMultiplier);

            return passiveMobsterAmount;
        }

        [ContextMenu("GameManager/EndGame")]
        private void EndGame()
        {
            Debug.Log("Game Over");
            OnGameOver?.Invoke(currentOutcome);
        }
        #endregion
    }
}
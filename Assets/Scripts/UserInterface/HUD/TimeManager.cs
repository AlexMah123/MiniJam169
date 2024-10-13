using System;
using System.Collections;
using Game;
using UnityEngine;
using TMPro;

namespace UserInterface.HUD
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI dayText;
        [SerializeField] private TextMeshProUGUI timeText;

        private int _currentTime;
        
        private void OnDisable()
        {
            GameManager.Instance.OnOutcomeChanged -= UpdateTime;
        }

        private void Start()
        {
            GameManager.Instance.OnOutcomeChanged += UpdateTime;
        }

        private void UpdateTime(GameOutcome outcome, int maxPoliceAlert, int maxMobsterAlert)
        {
            dayText.text = $"Day: {outcome.day}";
            timeText.text = $"Remaining: {outcome.timeRemaining} Hours";
        }
    }

}
using System;
using Game;
using UnityEngine;
using TMPro;

namespace UserInterface.HUD
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI timeText;

        private void OnDisable()
        {
            GameManager.Instance.OnOutcomeChanged -= UpdateTime;
        }

        private void Start()
        {
            GameManager.Instance.OnOutcomeChanged += UpdateTime;
        }

        private void UpdateTime(GameOutcome outcome)
        {
            timeText.text = $"Remaining: {outcome.timeRemaining} Hours";
        }
    }

}
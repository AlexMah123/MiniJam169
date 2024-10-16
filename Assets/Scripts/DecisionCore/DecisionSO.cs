using System;
using UnityEngine;

namespace DecisionCore
{
    [Serializable]
    public struct DecisionOutcome
    {
        public int timeRequired;
        public int policeAlertRaised;
        public int mobsterAlertRaised;
        public int mobsterCaught;
    }
    
    [CreateAssetMenu(menuName = "Scriptables/DecisionSO", order = 0)]
    public class DecisionSO : ScriptableObject
    {
        public Sprite icon;
        public string decisionName;
        public string decisionDescription;
        
        [TextArea(1, 5)]
        public string decisionEffect;
        
        public DecisionOutcome decisionOutcome;
    }
}


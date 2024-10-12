using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Decision
{
    [Serializable]
    public struct DecisionOutcome
    {
        public int cost;
        public int timeRequired;
        public int policeAlertRaised;
        public int mobsterAlertRaised;
        public int mobsterCaught;
    }
    
    [CreateAssetMenu(menuName = "Scriptables/DecisionSO", order = 0)]
    public class DecisionSO : ScriptableObject
    {
        public string decisionName;
        public string decisionDescription;
        
        [TextArea(1, 5)]
        public string decisionEffect;
        
        public DecisionOutcome decisionOutcome;
    }
}


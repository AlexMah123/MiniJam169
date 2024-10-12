using System;
using TMPro;
using UnityEngine;

namespace UserInterface.HUD.Tooltip
{
    public class TooltipManager : MonoBehaviour
    {
        public static TooltipManager Instance;

        public TextMeshProUGUI textComponent;
        
        private void Awake()
        {
            if(Instance != this && Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            
            gameObject.SetActive(false);

        }

        private void Start()
        {
            Cursor.visible = true;
        }
        
        private void Update()
        {
            transform.position = Input.mousePosition;
        }
        
        public void ShowTooltip(string text)
        {
            gameObject.SetActive(true);
            textComponent.text = text;
        }
    
        public void HideTooltip()
        {
            gameObject.SetActive(false);
            textComponent.text = string.Empty;
        }
    }
}
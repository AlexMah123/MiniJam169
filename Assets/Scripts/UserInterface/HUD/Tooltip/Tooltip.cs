using UnityEngine;
using UnityEngine.EventSystems;

namespace UserInterface.HUD.Tooltip
{
    public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string tooltipText;


        public void OnPointerEnter(PointerEventData eventData)
        {
            TooltipManager.Instance.ShowTooltip(tooltipText);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Instance.HideTooltip();
        }
    }
}
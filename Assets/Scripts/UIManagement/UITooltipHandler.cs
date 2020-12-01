using UnityEngine;
using UnityEngine.EventSystems;

namespace UIManagement
{
    public class UITooltipHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private string title;
        [SerializeField] [TextArea(3,10)] private string description;

        private RectTransform rectTransform;
        
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            UITooltip.Instance.Show(rectTransform.anchoredPosition, title, description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UITooltip.Instance.Hide();
        }
    }
}
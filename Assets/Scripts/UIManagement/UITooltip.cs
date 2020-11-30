using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    public class UITooltip : MonoBehaviour
    {
        public static UITooltip Instance;
        
        [SerializeField] private RectTransform frame;
        [SerializeField] private Text titleText;
        [SerializeField] private Text descriptionText;

        private void Start()
        {
            Instance = this;
        }

        public void Show(Vector2 position, string title, string description)
        {
            frame.gameObject.SetActive(true);
            frame.anchoredPosition = position + new Vector2(35 - 90, 50 + 90);
            titleText.text = title;
            descriptionText.text = description;
        }

        public void Hide()
        {
            frame.gameObject.SetActive(false);
        }
    }
}
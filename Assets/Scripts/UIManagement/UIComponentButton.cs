using System;
using JetBrains.Annotations;
using ShipManagement;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIManagement
{
    public class UIComponentButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private int componentTypeID;

        private RectTransform rectTransform;
        
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        [UsedImplicitly]
        public void OnClick()
        {
            ShipBuilder.Instance.SetCurrentComponent(componentTypeID);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ShipComponent component = ShipBuilder.Instance.GetComponentByTypeID(componentTypeID);
            UITooltip.Instance.Show(rectTransform.anchoredPosition, component.Name, component.Description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            UITooltip.Instance.Hide();
        }
    }
}
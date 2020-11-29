using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ShipManagement;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    public class UIShipBuilder : MonoBehaviour
    {
        [SerializeField] private List<Button> buttons;

        private float startY;
        private float targetY = 0;

        private void Start()
        {
            startY = transform.localPosition.y;
        }

        private void Update()
        {
            transform.localPosition = new Vector3(0, Mathf.Lerp(transform.localPosition.y, startY + targetY, Time.deltaTime * 5f));
        }

        [UsedImplicitly]
        public void SelectComponent(int typeIndex)
        {
            ShipBuilder.Instance.SetCurrentComponent(typeIndex);
        }

        [UsedImplicitly]
        public void Clear()
        {
            ShipBuilder.Instance.Clear();
        }

        public void Refesh(Player player)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].interactable = player.DiscoveredComponents.Contains(i + 1);
            }
        }

        public void Show()
        {
            targetY = 0;
        }

        public void Hide()
        {
            targetY = -110;
        }
    }
}
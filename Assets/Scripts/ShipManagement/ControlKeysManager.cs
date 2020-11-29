using ShipManagement.Components;
using UIManagement;
using UnityEngine;

namespace ShipManagement
{
    public class ControlKeysManager : MonoBehaviour
    {
        public static ControlKeysManager Instance;

        [SerializeField] private UIControlledComponentInfo info;
        [SerializeField] private GameObject selectedFrame;

        private ControlledComponent selectedComponent;
        private bool readKeyMode = false;
    
        private readonly KeyCode[] nonLetterKeys = 
        {
            KeyCode.Space, KeyCode.LeftControl, KeyCode.RightControl, KeyCode.Tab, KeyCode.Space, KeyCode.LeftShift,
            KeyCode.RightShift, KeyCode.LeftAlt, KeyCode.RightAlt
        };
    
        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            if (!readKeyMode || selectedComponent == null)
                return;

            KeyCode? key = TryGetKey();

            if (key != null)
            {
                selectedComponent.ControlKey = key.Value;
                readKeyMode = false;
                info.SetTarget(selectedComponent.ControlKey);
            }
        }

        private KeyCode? TryGetKey()
        {
            if (!Input.anyKeyDown)
                return null;

            for (KeyCode i = KeyCode.A; i <= KeyCode.Z; i++)
            {
                if (Input.GetKeyDown(i))
                    return i;
            }

            foreach (KeyCode i in nonLetterKeys)
            {
                if (Input.GetKeyDown(i))
                    return i;
            }

            return null;
        }

        public void ReadKey()
        {
            if (readKeyMode)
            {
                readKeyMode = false;
                info.SetTarget(selectedComponent.ControlKey);
            }
            else
            {
                readKeyMode = true;
                info.ReadKeyMode();
            }
        }

        public void ShowInfo()
        {
            readKeyMode = false;
            info.NoTarget();
        }

        public void HideInfo()
        {
            readKeyMode = false;
            selectedComponent = null;
            selectedFrame.SetActive(false);
        }

        public void SelectComponent(ControlledComponent component)
        {
            if (selectedComponent == component)
                return;

            selectedComponent = component;
            info.SetTarget(selectedComponent.ControlKey);
            selectedFrame.transform.position = component.transform.position;
            selectedFrame.SetActive(true);
        }

        public void DeselectIsEquals(ShipComponent component)
        {
            if (component == selectedComponent)
                Deselect();
        }

        public void Deselect()
        {
            selectedComponent = null;
            selectedFrame.SetActive(false);
            info.NoTarget();
        }
    }
}
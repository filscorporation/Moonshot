using UnityEngine;

namespace UIManagement
{
    public class UISimulateButton : MonoBehaviour
    {
        [SerializeField] private GameObject simulateButton;
        [SerializeField] private GameObject buildButton;
        
        public void Hide()
        {
            simulateButton.SetActive(false);   
            buildButton.SetActive(false);   
        }

        public void ShowSimulateButton()
        {
            simulateButton.SetActive(true);   
            buildButton.SetActive(false);   
        }

        public void ShowBuildButton()
        {
            simulateButton.SetActive(false);   
            buildButton.SetActive(true);   
        }
    }
}
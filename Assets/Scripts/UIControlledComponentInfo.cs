using UnityEngine;
using UnityEngine.UI;

public class UIControlledComponentInfo : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Button changeButton;
    [SerializeField] private Text buttonText;

    private const string NO_TARGET_TEXT = "Select component to set key";
    private const string TARGET_TEXT = "Current control key: {0}";
    private const string READ_KEY_TEXT = "Press any (almost) key";
    private const string CHANGE_TEXT = "Change";
    private const string CANCEL_TEXT = "Cancel";
    
    public void NoTarget()
    {
        titleText.text = NO_TARGET_TEXT;
        changeButton.interactable = false;
        buttonText.text = CHANGE_TEXT;
    }

    public void SetTarget(KeyCode key)
    {
        titleText.text = string.Format(TARGET_TEXT, key);
        changeButton.interactable = true;
        buttonText.text = CHANGE_TEXT;
    }

    public void ReadKeyMode()
    {
        titleText.text = READ_KEY_TEXT;
        buttonText.text = CANCEL_TEXT;
    }
}
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    public class UITutorialTooltip : MonoBehaviour
    {
        [SerializeField] private GameObject tooltipObject;
        [SerializeField] private Text text;
        [SerializeField] private Text tooltipsProgress;

        private enum TutorialEvents
        {
            Start = 0,
            FirstComponent = 1,
            Scrap = 2,
            Blueprints = 3,
            Tougness = 4,
            ControlKeys = 5,
        }
        
        private readonly Dictionary<TutorialEvents, string> tutorialTexts = new Dictionary<TutorialEvents, string>
        {
            { TutorialEvents.Start, "Welcome to Reach The Moon. \nYour goal is to build your own ship from components and fly high enough to reach the moon" },
            { TutorialEvents.FirstComponent, "Attach your first component to your ship and press start.\n" +
                                             "Click baloon at bottom menu and click above existing cabin block.\n" +
                                             "You can alwas delete placed block with right mouse button" },
            { TutorialEvents.Scrap, "Scrap used to add components to the ship. On the left you can see how much your current ship costs, " +
                                    "if this number is above what you collected - you won't be able to start.\n" +
                                    "Explore the map to find more scrap" },
            { TutorialEvents.Blueprints, "To unlock new components you will need to find blueprints.\nExplore map to the left and right to find them" },
            { TutorialEvents.Tougness, "Each components will affect overall weight and toughness of your ship.\n" +
                                       "When you will fall or hit the walls - you will take damage.\n" +
                                       "When damage taken is more than toughness - your ship will be destroyed and you will go back to building" },
            { TutorialEvents.ControlKeys, "For better control over your ship you can set any custom key to control components, default ones depends on component role and rotation" },
        };

        private TutorialEvents currentTooltip = TutorialEvents.Start;

        private void Start()
        {
            StartCoroutine(StartTutorials());
        }

        private IEnumerator StartTutorials()
        {
            yield return new WaitForSeconds(2f);
            ShowTooltip();
        }

        private void ShowTooltip()
        {
            if (!tutorialTexts.ContainsKey(currentTooltip))
            {
                tooltipObject.SetActive(false);
                return;
            }
            
            tooltipObject.SetActive(true);
            text.text = tutorialTexts[currentTooltip];
            tooltipsProgress.text = $"{(int) currentTooltip + 1}/{tutorialTexts.Count}";
        }

        [UsedImplicitly]
        public void Next()
        {
            currentTooltip++;
            ShowTooltip();
        }

        [UsedImplicitly]
        public void Restart()
        {
            currentTooltip = TutorialEvents.Start;
            ShowTooltip();
        }
    }
}
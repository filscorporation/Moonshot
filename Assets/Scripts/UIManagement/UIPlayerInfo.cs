using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    public class UIPlayerInfo : MonoBehaviour
    {
        private Player player;

        [SerializeField] private Text toughnessText;
        [SerializeField] private Text damageText;
        [SerializeField] private Text altitudeText;
        [SerializeField] private Text scrapText;

        private void Update()
        {
            if (player == null)
            {
                player = GameManager.Instance.Player;
                if (player == null)
                    return;
            }

            UpdateToughnessText();
            damageText.text = $"Damage: {player.Ship.DamageTaken.ToString("F1", CultureInfo.InvariantCulture)}";
            altitudeText.text = $"Altitude: {player.Ship.Components.First().transform.position.y.ToString("F1", CultureInfo.InvariantCulture)}";
            scrapText.text = $"Scrap: {player.Scrap}";
        }

        private void UpdateToughnessText()
        {
            int t = player.Ship.Toughness;
            toughnessText.text = $"Toughness: {t}";
            toughnessText.color = t >= 0 ? Color.green : Color.red;
        }
    }
}
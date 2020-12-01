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
        [SerializeField] private Text shipCostText;
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
            UpdateShipCostText();
            damageText.text = $"Damage: {player.Ship.DamageTaken.ToString("F1", CultureInfo.InvariantCulture)}";
            altitudeText.text = $"Altitude: {player.Ship.Altitude.ToString("F1", CultureInfo.InvariantCulture)}/{Moon.MOON_HEIGHT}";
            scrapText.text = $"Scrap: {player.Scrap}";
        }

        private void UpdateToughnessText()
        {
            int t = player.Ship.Toughness;
            toughnessText.text = $"Toughness: {t}";
            toughnessText.color = t >= 0 ? Color.green : Color.red;
        }

        private void UpdateShipCostText()
        {
            int c = player.Ship.Cost;
            shipCostText.text = $"Ship cost: {c}";
            shipCostText.color = c <= player.Scrap ? Color.green : Color.red;
        }
    }
}
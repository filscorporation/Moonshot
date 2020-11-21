using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    private Player player;

    [SerializeField] private Text toughnessText;
    [SerializeField] private Text damageText;
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
        scrapText.text = $"Scrap: {player.Scrap}";
    }

    private void UpdateToughnessText()
    {
        int t = player.Ship.Toughness;
        toughnessText.text = $"Toughness: {t}";
        toughnessText.color = t >= 0 ? Color.green : Color.red;
    }
}
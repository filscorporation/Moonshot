using System.Collections;
using System.Linq;
using JetBrains.Annotations;
using ShipManagement;
using UnityEngine;
using UnityEngine.UI;
using WorldManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject exitScreen;
    [SerializeField] private GameObject winningScreen;
    [SerializeField] private Text scrapText;

    private bool isWon = false;
    private int maxScrap;
    
    public Player Player { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Player = new Player(ShipBuilder.Instance.Initialize());
        maxScrap = FindObjectsOfType<Scrap>().Sum(s => s.ScrapAmount);
    }

    private void Update()
    {
        if (isWon)
            return;
        
        if (Player?.Ship != null)
        {
            if (Player.Ship.Altitude >= Moon.MOON_HEIGHT)
                StartCoroutine(Win());
        }
    }

    private IEnumerator Win()
    {
        isWon = true;
        int scrapLeft = FindObjectsOfType<Scrap>().Sum(s => s.ScrapAmount);
        
        yield return new WaitForSeconds(1f);

        Time.timeScale = 0;
        winningScreen.SetActive(true);
        scrapText.text = $"Scrap collected: {maxScrap - scrapLeft}/{maxScrap}";
    }

    [UsedImplicitly]
    public void Continue()
    {
        winningScreen.SetActive(false);
        Time.timeScale = 1;
    }

    [UsedImplicitly]
    public void Exit()
    {
        exitScreen.SetActive(true);
        Time.timeScale = 0;
    }

    [UsedImplicitly]
    public void ExitConfirm()
    {
        Application.Quit();
    }

    [UsedImplicitly]
    public void ExitCancel()
    {
        exitScreen.SetActive(false);
        Time.timeScale = 1;
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public Player Player { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Player = new Player(ShipBuilder.Instance.Initialize());
    }
}

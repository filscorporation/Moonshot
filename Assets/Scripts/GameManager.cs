using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        ShipBuilder.Instance.Initialize();
    }
}

using UnityEngine;

public class Moon : MonoBehaviour
{
    public const float MOON_HEIGHT = 150f;
    
    private void Update()
    {
        if (GameManager.Instance.Player.Ship != null)
            transform.position = new Vector3(GameManager.Instance.Player.Ship.PositionX + 8f, MOON_HEIGHT);
    }
}
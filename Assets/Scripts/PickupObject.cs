using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public abstract class PickupObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        ShipComponent component = other.gameObject.GetComponent<ShipComponent>();
        if (component != null)
        {
            component.Ship.PickUp(this);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
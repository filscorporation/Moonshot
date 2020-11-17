using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;

    private void FixedUpdate()
    {
        if (target == null)
            target = FindObjectOfType<Cabin>().transform;

        if (target != null)
        {
            Vector2 position = Vector2.Lerp(transform.position, target.transform.position, Time.fixedDeltaTime * 5f);
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }
    }
}
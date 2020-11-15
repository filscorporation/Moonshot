using UnityEngine;

public class Thrusters : Component
{
    [SerializeField] private float force = 10f;

    private Rigidbody2D body;
    
    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!IsEnabled)
            return;
        
        if (Input.GetKey(KeyCode.W))
        {
            ApplyForce();
        }
    }

    private void ApplyForce()
    {
        body.AddForce(transform.up * force, ForceMode2D.Force);
    }
}
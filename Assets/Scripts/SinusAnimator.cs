using UnityEngine;
using Random = UnityEngine.Random;

public class SinusAnimator : MonoBehaviour
{
    [SerializeField] [Range(0.01f, 2f)] private float amplitude = 0.3f;
    [SerializeField] [Range(0.01f, 2f)] private float speed = 1f;

    private float startTime;
    private Vector2 startPosition;
    
    private void Start()
    {
        startTime = Random.Range(0, 1 / speed);
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = startPosition + new Vector2(0, Mathf.Sin((startTime + Time.time) * speed) * amplitude); 
    }
}
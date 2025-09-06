using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField, Tooltip("Speed of this bullet.")]
    private float _speed = 4f;
    
    [SerializeField, Tooltip("Normalized direction of this bullet.")]
    private Vector3 _direction = Vector3.zero;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition += _direction * (_speed * Time.deltaTime);
        transform.position = newPosition;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
        _direction.x += _speed;
        transform.LookAt(transform.position + _direction);
    }
}

using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] [Tooltip("Speed of this bullet.")]
    private float _speed = 4f;

    [SerializeField] [Tooltip("Normalized direction of this bullet.")]
    private Vector3 _direction = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        var newPosition = transform.position;
        newPosition += _direction * (_speed * Time.deltaTime);
        transform.position = newPosition;
    }

    public void SetDirection(Vector3 direction)
    {
        _direction = direction;
        _direction.x += _speed;
        transform.LookAt(transform.position + _direction);
    }

    private void OnCollisionEnter(Collision other)
    {
        var target = other.gameObject;
        Debug.unityLogger.Log("Bullet hit " + target);
        
        if (target.name != "Player")
        {

            if (target.name.Contains("EnemyObj_Spikes"))
            {
                Destroy(target);
            }
            
            // Doesn't matter what bullet hit, if bullet hit something, needs to be 
            // destroyed.
            if (!target.name.Contains("Spawn"))
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject;
        Debug.unityLogger.Log("Bullet hit " + target);
        
        if (target.name != "Player")
        {
            if (target.name.Contains("EnemyObj_Spikes"))
            {
                VFXHandler vfxHandler = target.GetComponent<VFXHandler>();
                vfxHandler?.SpawnExplosion();
                Destroy(target);
                
            }
            // Doesn't matter what bullet hit, if bullet hit something, needs to be 
            // destroyed.
            if (!target.name.Contains("Spawn"))
                Destroy(gameObject);
        }
    }
}
using System.Collections.Generic;
using Contracts;
using UnityEngine;
using System.Linq;

public class Bullet : MonoBehaviour, IBullet
{
    [SerializeField] [Tooltip("Speed of this bullet.")]
    private float _speed = 4f;

    [SerializeField] [Tooltip("Normalized direction of this bullet.")]
    private Vector3 _direction = Vector3.zero;

    private readonly IReadOnlyCollection<string> _canDestroy = new[]
    {
        "EnemyObj_Spikes"
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (_direction.sqrMagnitude > 0f) transform.position += _direction * (_speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        var target = other.gameObject;

        if (!_canDestroy.Any(item => target.name.Contains(item))) return;
        
        Debug.unityLogger.Log("Bullet hit " + target);
        
        IVFXHandler vfxHandler = target.GetComponent<VFXHandler>();
        vfxHandler?.SpawnExplosion();
        Destroy(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject;

        if (!_canDestroy.Any(item => target.name.Contains(item))) return;
        
        Debug.unityLogger.Log("Bullet hit " + target);
        
        IVFXHandler vfxHandler = target.GetComponent<VFXHandler>();
        vfxHandler?.SpawnExplosion();

        Destroy(target);
    }

    public void SetDirection(Vector3 direction)
    {
        // _direction.y = 0f;

        if (direction.sqrMagnitude <= Mathf.Epsilon)
            return;

        _direction = direction.normalized;

        transform.rotation = Quaternion.LookRotation(_direction, Vector3.up);

        transform.LookAt(transform.position + _direction);
    }
}
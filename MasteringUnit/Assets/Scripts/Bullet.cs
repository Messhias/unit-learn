using System.Collections.Generic;
using Contracts;
using UnityEngine;
using System.Linq;

public class Bullet : MonoBehaviour, IBullet
{
    #region *** Editor fields ***
    
    [SerializeField] [Tooltip("Speed of this bullet.")]
    private float _speed = 4f;

    [SerializeField] [Tooltip("Normalized direction of this bullet.")]
    private Vector3 _direction = Vector3.zero;

    #endregion
    
    #region *** private properties ***

    private IReadOnlyCollection<string> _canDestroy = new []
    {
        "EnemyObj_Spikes",
    };
    
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (_direction.sqrMagnitude > 0f)
        {
            transform.position += _direction * (_speed * Time.deltaTime);
        }
    }

    public void SetDirection(Vector3 direction)
    {
        // direction.y = 0f;

        if (direction.sqrMagnitude <= Mathf.Epsilon)
            return;

        _direction = direction.normalized;

        transform.rotation = Quaternion.LookRotation(_direction, Vector3.up);

        transform.LookAt(transform.position + _direction);
    }

    private void OnCollisionEnter(Collision other)
    {
        var target = other.gameObject;
        Debug.unityLogger.Log("Bullet hit " + target);

        if (!_canDestroy.Contains(target.name)) return;
        
        IVFXHandler vfxHandler = target.GetComponent<VFXHandler>();
        vfxHandler?.SpawnExplosion();
        
        // apply knockback when damage is dealt
        var rb = other.gameObject?.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddExplosionForce(100, transform.position, 10f);
        
        Destroy(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        
        var target = other.gameObject;
        Debug.unityLogger.Log("Bullet hit " + target);

        if (!_canDestroy.Contains(target.name)) return;
        
        IVFXHandler vfxHandler = target.GetComponent<VFXHandler>();
        vfxHandler?.SpawnExplosion();
        
        // apply knockback when damage is dealt
        var rb = other.gameObject?.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddExplosionForce(100, transform.position, 10f);
        
        Destroy(target);
    }
}
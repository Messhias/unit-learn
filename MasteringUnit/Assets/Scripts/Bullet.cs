using Contracts;
using UnityEngine;
using System.Linq;
using Implementations;

public class Bullet : MonoBehaviour, IBullet
{
    [SerializeField] [Tooltip("Speed of this bullet.")]
    private float _speed = 4f;

    [SerializeField] [Tooltip("Normalized direction of this bullet.")]
    private Vector3 _direction = Vector3.zero;

    private readonly ImmutableList<string> _noDestroyOnEnterIn = new()
    {
        "Player",
        "RangeWeapon",
        "Spawn",
    };

    private readonly ImmutableList<string> _destroyOnEnterIn = new()
    {
        "EnemyObj_Spikes",
    };

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

        if (_noDestroyOnEnterIn.Any(item => target.name.Contains(item)) &&
            _noDestroyOnEnterIn.Any(item => target.CompareTag(item))) return;
        
        if (!_destroyOnEnterIn.Any(item => target.name.Contains(item))) return;
        
        IVFXHandler vfxHandler = target.GetComponent<VFXHandler>();
        vfxHandler?.SpawnExplosion();
        Destroy(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject;
        Debug.unityLogger.Log("Bullet hit " + target);

        if (_noDestroyOnEnterIn.Any(item => target.name.Contains(item)) ||
            !_noDestroyOnEnterIn.Any(item => target.CompareTag(item))) return;

        if (!_destroyOnEnterIn.Any(item => target.name.Contains(item))) return;
        
        IVFXHandler vfxHandler = target.GetComponent<VFXHandler>();
        vfxHandler?.SpawnExplosion();
        Destroy(target);
    }
}
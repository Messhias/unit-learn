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

    private readonly ImmutableList<string> _cannotDestroy = new()
    {
        "Player",
        "RangeWeapon",
        "Spawn",
    };

    private readonly ImmutableList<string> _canDestroy = new()
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
        if (bulletDirection.sqrMagnitude > 0f) transform.position += bulletDirection * (speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        var target = other.gameObject;
        Debug.unityLogger.Log("Bullet hit " + target);

        if (_cannotDestroy.Any(item => target.name.Contains(item)) &&
            _cannotDestroy.Any(item => target.CompareTag(item))) return;
        
        if (!_canDestroy.Any(item => target.name.Contains(item))) return;
        
        IVFXHandler vfxHandler = target.GetComponent<VFXHandler>();
        vfxHandler?.SpawnExplosion();
        Destroy(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject;
        Debug.unityLogger.Log("Bullet hit " + target);

        if (_cannotDestroy.Any(item => target.name.Contains(item)) ||
            !_cannotDestroy.Any(item => target.CompareTag(item))) return;

        if (!_canDestroy.Any(item => target.name.Contains(item))) return;
        
        IVFXHandler vfxHandler = target.GetComponent<VFXHandler>();
        vfxHandler?.SpawnExplosion();

        // apply knockback when damage is dealt
        var rb = other.gameObject?.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddExplosionForce(100, transform.position, 10f);

        Destroy(target);
    }

    public void SetDirection(Vector3 direction)
    {
        // bulletDirection.y = 0f;

        if (direction.sqrMagnitude <= Mathf.Epsilon)
            return;

        bulletDirection = direction.normalized;

        transform.rotation = Quaternion.LookRotation(bulletDirection, Vector3.up);

        transform.LookAt(transform.position + bulletDirection);
    }

    #region *** Editor fields ***

    [SerializeField] [Tooltip("Speed of this bullet.")]
    private float speed = 4f;

    [SerializeField] [Tooltip("Normalized direction of this bullet.")]
    private Vector3 bulletDirection = Vector3.zero;

    #endregion
}
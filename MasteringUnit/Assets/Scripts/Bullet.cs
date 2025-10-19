using System.Collections.Generic;
using System.Linq;
using Contracts;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour, IBullet
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (direction.sqrMagnitude > 0f) transform.position += direction * (speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        var target = other.gameObject;

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

        if (!_canDestroy.Contains(target.name)) return;

        IVFXHandler vfxHandler = target.GetComponent<VFXHandler>();
        vfxHandler?.SpawnExplosion();

        Destroy(target);
    }

    public void SetDirection(Vector3 direction)
    {
        // _direction.y = 0f;

        if (direction.sqrMagnitude <= Mathf.Epsilon)
            return;

        this.direction = direction.normalized;

        transform.rotation = Quaternion.LookRotation(this.direction, Vector3.up);

        transform.LookAt(transform.position + this.direction);
    }

    #region *** private properties ***

    [FormerlySerializedAs("_direction")] [SerializeField] [Tooltip("Normalized direction of this bullet.")]
    private Vector3 direction = Vector3.zero;

    [FormerlySerializedAs("_speed")] [SerializeField] [Tooltip("Bullet speed")]
    private float speed = 400.0f;

    private readonly IReadOnlyCollection<string> _canDestroy = new[]
    {
        "EnemyObj_Spikes"
    };

    #endregion
}
using System.Collections.Generic;
using System.Linq;
using Contracts;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    #region *** private properties ***

    private readonly IReadOnlyCollection<string> _canDestroy = new[]
    {
        "EnemyObj_Spikes"
    };

    #endregion

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
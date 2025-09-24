using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HealthModifier : MonoBehaviour
{
    private enum DamageTarget
    {
        Enemies,
        Player,
        All,
        None
    }

    #region *** Editor ***

    [SerializeField] [Tooltip("Knockback force when this damage is applied.")]
    private float _knockbackForce;

    [SerializeField] [Tooltip("The class of object that should be damaged.")]
    private float _healthChange;

    [SerializeField] [Tooltip("The class of object that should be damaged.")]
    private DamageTarget _applyToTarget = DamageTarget.Player;

    [SerializeField] [Tooltip("Should object self-destruct on collision?")]
    private bool _destroyOnCollision;

    #endregion
    
    #region *** private ***

    private readonly IReadOnlyCollection<string> _cantKnockIn = new[]
    {
        "Player",
        "RangeWeapon",
        "Spawn",
        "Plane",
        "Ground"
    };

    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnCollisionStay(Collision other)
    {
        if (!(_healthChange < 0f) || _knockbackForce == 0) return;

        if (_cantKnockIn.Contains(other.gameObject.name) ||
            _cantKnockIn.Contains(other.gameObject.tag))
            return;

        // apply knockback when damage is dealt
        var rb = other.gameObject?.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddExplosionForce(_knockbackForce, transform.position, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        var hitObject = other.gameObject;

        // get HealthManager of the object we've hit.
        var healthManager = hitObject.GetComponent<HealthManager>();

        if (healthManager && IsValidTarget(hitObject)) healthManager.AdjustCurrentHealth(_healthChange);

        if (_destroyOnCollision) Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!(_healthChange < 0f) || _knockbackForce == 0) return;

        if (_cantKnockIn.Contains(other.name) || _cantKnockIn.Contains(other.tag))
            return;

        // apply knockback when damage is dealt
        var rb = other?.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddExplosionForce(_knockbackForce, transform.position, 10f);
    }

    private bool IsValidTarget(GameObject possibleTarget)
    {
        switch (_applyToTarget)
        {
            case DamageTarget.All:
                return true;
            case DamageTarget.None:
                return false;
            case DamageTarget.Player when possibleTarget.GetComponent<PlayerController>():
            case DamageTarget.Enemies when possibleTarget.GetComponent<AIBrain>():
                return true;
            default:
                // not valid target at all.
                return false;
        }
    }
}
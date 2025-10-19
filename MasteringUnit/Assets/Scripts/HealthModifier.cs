using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class HealthModifier : MonoBehaviour
{
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

    private void OnCollisionStay(Collision other)
    {
        if (!(healthChange < 0f) || knockbackForce == 0) return;

        if (_cantKnockIn.Contains(other.gameObject.name) ||
            _cantKnockIn.Contains(other.gameObject.tag))
            return;

        // apply knockback when damage is dealt
        var rb = other.gameObject?.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddExplosionForce(knockbackForce, transform.position, 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        var hitObject = other.gameObject;

        // get HealthManager of the object we've hit.
        var healthManager = hitObject.GetComponent<HealthManager>();

        if (healthManager && IsValidTarget(hitObject)) healthManager.AdjustCurrentHealth(healthChange);

        if (destroyOnCollision) Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!(healthChange < 0f) || knockbackForce == 0) return;

        if (_cantKnockIn.Contains(other.name) || _cantKnockIn.Contains(other.tag))
            return;

        // apply knockback when damage is dealt
        var rb = other?.GetComponent<Rigidbody>();
        if (rb != null)
            rb.AddExplosionForce(knockbackForce, transform.position, 10f);
    }

    private bool IsValidTarget(GameObject possibleTarget)
    {
        switch (applyToTarget)
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

    private enum DamageTarget
    {
        Enemies,
        Player,
        All,
        None
    }

    #region *** Editor ***

    [FormerlySerializedAs("_knockbackForce")] [SerializeField] [Tooltip("Knockback force when this damage is applied.")]
    private float knockbackForce;

    [FormerlySerializedAs("_healthChange")] [SerializeField] [Tooltip("The class of object that should be damaged.")]
    private float healthChange;

    [FormerlySerializedAs("_applyToTarget")] [SerializeField] [Tooltip("The class of object that should be damaged.")]
    private DamageTarget applyToTarget = DamageTarget.Player;

    [FormerlySerializedAs("_destroyOnCollision")]
    [SerializeField]
    [Tooltip("Should object self-destruct on collision?")]
    private bool destroyOnCollision;

    #endregion
}
using UnityEngine;

public class HealthModifier : MonoBehaviour
{
    public enum DamageTarget
    {
        Enemies,
        Player,
        All,
        None
    }

    [SerializeField] [Tooltip("The class of object that should be damaged.")]
    private float _healthChange;

    [SerializeField] [Tooltip("The class of object that should be damaged.")]
    private DamageTarget _applyToTarget = DamageTarget.Player;

    [SerializeField] [Tooltip("Should object self-destruct on collision?")]
    private bool _destroyOnCollision;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        var hitObject = other.gameObject;

        // get HealthManager of the object we've hit.
        var healthManager = hitObject.GetComponent<HealthManager>();

        if (healthManager && IsValidTarget(hitObject)) healthManager.AdjustCurrentHealth(_healthChange);

        if (_destroyOnCollision) Destroy(gameObject);
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
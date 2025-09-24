using UnityEngine;

public class PhysicsForceZone : MonoBehaviour
{
    [SerializeField] [Tooltip("Force applied to any hit RigidBody object.")]
    private float forceToApply = 1;

    private void Awake()
    {
        var c = GetComponent<CapsuleCollider>();

        if (c) c.isTrigger = true;

        var rb = GetComponent<Rigidbody>();

        if (!rb) return;

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        var hitObject = other.gameObject;

        if (hitObject == null) return;

        var rb = hitObject.GetComponent<Rigidbody>();

        // get the direction of the Y axis.
        var direction = transform.up;

        rb.AddForce(direction * forceToApply);
    }
}
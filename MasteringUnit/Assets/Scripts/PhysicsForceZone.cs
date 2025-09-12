using System;
using UnityEngine;

public class PhysicsForceZone : MonoBehaviour
{
    [SerializeField, Tooltip("Force applied to any hit RigidBody object.")]
    private float _forceToApply = 1;

    private void Awake()
    {
        CapsuleCollider c =  GetComponent<CapsuleCollider>();

        if (c)
        {
            c.isTrigger = true;
        }
        
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject hitObject = other.gameObject;

        if (hitObject != null)
        {
            Rigidbody rb = hitObject.GetComponent<Rigidbody>();
            
            // get the direction of the Y axis.
            Vector3 direction = transform.up;
            
            rb.AddForce(direction * _forceToApply);
        }
    }
}

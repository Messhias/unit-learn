using System;
using UnityEngine;

public class Springboard : MonoBehaviour
{
    [SerializeField, Tooltip("Velocity change on the Y axis.")]
    private float _upwardsForce = 2000f;

    private void OnCollisionEnter(Collision other)
    {
        GameObject hitObject = other.gameObject;
        Rigidbody rb = hitObject?.GetComponent<Rigidbody>();
        rb?.AddForce(0, _upwardsForce, 0);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
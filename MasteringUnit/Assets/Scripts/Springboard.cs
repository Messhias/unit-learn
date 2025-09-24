using UnityEngine;

public class Springboard : MonoBehaviour
{
    [SerializeField] [Tooltip("Velocity change on the Y axis.")]
    private float upwardsForce = 2000f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnCollisionEnter(Collision other)
    {
        var hitObject = other.gameObject;
        var rb = hitObject?.GetComponent<Rigidbody>();
        rb?.AddForce(0, upwardsForce, 0);
    }
}
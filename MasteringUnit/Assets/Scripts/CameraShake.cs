using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] [Tooltip("Magnitude of the shake effect.")]
    private float _shake = 0.05f;

    internal Vector3 _newPosition = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        // if enabled, give camera a little shake
        _newPosition = new Vector3
        {
            x = Random.Range(-_shake, +_shake),
            y = Random.Range(-_shake, +_shake),
            z =  Random.Range(-_shake, +_shake)
        };

        // transform.position = _newPosition;
    }
}
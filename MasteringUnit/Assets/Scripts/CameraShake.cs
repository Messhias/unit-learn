using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField, Tooltip("Magnitude of the shake effect.")]
    private float _shake = 0.05f;
    
    private Vector3 _startPosition;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // let's store the starting position.
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // if enabled, give camera a little shake
        var newPosition = new Vector3();
        newPosition.x = _startPosition.x + Random.Range(-_shake, +_shake);
        newPosition.y = _startPosition.y + Random.Range(-_shake, +_shake);
        newPosition.z = _startPosition.z + Random.Range(-_shake, +_shake);

        transform.position = newPosition;
    }
}

using UnityEngine;
using UnityEngine.Serialization;

public class CameraShake : MonoBehaviour
{
    [FormerlySerializedAs("_shake")] [SerializeField] [Tooltip("Magnitude of the shake effect.")]
    private float shake = 0.05f;

    internal Vector3 NewPosition = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        // if enabled, give camera a little shake
        NewPosition = new Vector3
        {
            x = Random.Range(-shake, +shake),
            y = Random.Range(-shake, +shake),
            z = Random.Range(-shake, +shake)
        };

        // transform.position = _newPosition;
    }
}
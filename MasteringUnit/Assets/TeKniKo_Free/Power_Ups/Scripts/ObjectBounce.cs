using UnityEngine;

public class ObjectBounce : MonoBehaviour
{
    public float bounceSpeed = 8;
    public float bounceAmplitude = 0.05f;
    public float rotationSpeed = 90;

    private float _startHeight;
    private float _timeOffset;

    // Start is called before the first frame update
    private void Start()
    {
        _startHeight = transform.localPosition.y;
        _timeOffset = Random.value * Mathf.PI * 2;
    }

    // Update is called once per frame
    private void Update()
    {
        //animate
        var finalheight = _startHeight + Mathf.Sin(Time.time * bounceSpeed + _timeOffset) * bounceAmplitude;
        var position = transform.localPosition;
        position.y = finalheight;
        transform.localPosition = position;

        //spin
        var rotation = transform.localRotation.eulerAngles;
        rotation.y += rotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
    }
}
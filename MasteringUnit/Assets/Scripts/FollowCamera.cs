using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] [Tooltip("The object to follow.")]
    private GameObject _target;

    [SerializeField] [Tooltip("Target offset.")]
    private Vector3 _targetOffset;

    [SerializeField] [Tooltip("The height off the ground to follow from.")]
    private float _camHeight = 9;

    [SerializeField] [Tooltip("The distance from the target to follow.")]
    private float _camDistance = -16;
    
    private CameraShake  _shake;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _shake = gameObject.GetComponent<CameraShake>();
    }

    // Update is called once per frame
    private void Update()
    {
        // if we do not have a target, let's stop.
        if (!_target)
            return;

        // get the actual target possition
        var targetPosition = _target.transform.position;
        
        // increment with our target offset.
        targetPosition += _targetOffset;
        
        // apply the camera height 
        targetPosition.y += _camHeight;
        
        // camera z position (profundity)
        targetPosition.z = _camDistance;

        // we're going to move smoothly the camera.
        var smoothPosition = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5.0f);

        // if our player got a damage, the camera start to shake.
        // on that we need to increment the new position (shake)
        // to our actual camera component.
        if (_shake.enabled)
        {
            smoothPosition += _shake._newPosition;
        }

        // apply the new position to the game object (Camera).
        transform.position = smoothPosition;
    }
}
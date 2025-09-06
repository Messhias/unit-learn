using UnityEngine;
using UnityEngine.Serialization;

public class PickUpItem : MonoBehaviour
{
    [FormerlySerializedAs("_rotationSpeed")] [SerializeField, Tooltip("The speed that this object rotates at.")]
    private float rotationSpeed = 5;

    private static int _sObjectsCollected = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // grab the current rotation, increment it, and re-apply it.
        Vector3 newRotation = transform.eulerAngles;
        
        newRotation.y += (rotationSpeed * Time.deltaTime);
        
        transform.eulerAngles = newRotation;
    }

    public void OnPickedUp(GameObject whoPickedUp)
    {
        // show the collection count in the console window
        _sObjectsCollected++;
        Debug.Log($"{_sObjectsCollected} items picked up");
        
        Destroy(gameObject);
    }
}

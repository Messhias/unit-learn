using UnityEngine;

public class GroundObjectController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // the speed that this object rotates.
        float speed = 15f;

        // get the current rotation of the ground object
        // based on the keys pressed
        // we will be altering this value and re-applying it.
        Vector3 newRotation = transform.localEulerAngles;

        // go through each of the direction keys: up, down, left, right
        // if any of them are pressed, alter the rotation of the ground plane.

        if (Input.GetKey(KeyCode.RightArrow))
        {
            newRotation.z -= Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newRotation.z += Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            newRotation.x +=  Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            newRotation.x -= Time.deltaTime * speed;
        }
        
        transform.localEulerAngles = newRotation;
    }
}
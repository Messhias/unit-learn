using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    // the rigid body physics component of this object
    // since we'll be accessing it a lot, we'll store it as member.
    private Rigidbody _rigidbody;

    // acceleration applied when directional input is received.
    [SerializeField, Tooltip("How much acceleration is applied to this object when direction input is received.")]
    private float movementAcceleration = 2;

    // the maximum velocity of this object.
    [SerializeField, Tooltip("The maximum velocity of this object - keeps the player from moving too fast.")]
    private float movementVelocityMax = 2;

    [SerializeField, Tooltip("Deceleration when no direction input is received.")]
    private float movementDeceleration = 0.1f;

    [FormerlySerializedAs("_jumpVelocity")] [SerializeField, Tooltip("Upwards force applied when jum kehy is pressed")]
    private float jumpVelocity = 20;

    [FormerlySerializedAs("_extraGravity")] [SerializeField, Tooltip("Additional gravitational pull")]
    private float extraGravity = 20;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        // get the current speed from the rigid body physics component.
        // grabbing this ensures we retain the gravity speed.
        Vector3 currentSpeed = _rigidbody.linearVelocity;

        // check to see if any of the keyboard arrows are pressed.
        // if so, adjust the speed of the player.
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentSpeed.x += (movementAcceleration * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentSpeed.x -= (movementAcceleration * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentSpeed.z += (movementAcceleration * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentSpeed.z -= (movementAcceleration * Time.deltaTime);
        }

        // does player want to jump?
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(currentSpeed.y) < 1)
        {
            currentSpeed.y += jumpVelocity;
        }
        else
        {
            currentSpeed.y -= extraGravity * Time.deltaTime;
        }

        // if both left and right keys are simultaneously pressed (or not pressed), apply friction
        if (Input.GetKey(KeyCode.LeftArrow) == Input.GetKey(KeyCode.RightArrow))
        {
            currentSpeed.x -= (movementDeceleration * currentSpeed.x);
        }

        if (Input.GetKey(KeyCode.UpArrow) == Input.GetKey(KeyCode.DownArrow))
        {
            currentSpeed.z -= (movementDeceleration * currentSpeed.z);
        }

        // apply max speed
        currentSpeed.x = Mathf.Clamp(currentSpeed.x, movementVelocityMax * -1, movementVelocityMax);
        currentSpeed.z = Mathf.Clamp(currentSpeed.z, movementVelocityMax * -1, movementVelocityMax);

        _rigidbody.linearVelocity = currentSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name} entered");
        // did we collide with the PickUpItem?
        if (!other.gameObject.GetComponent<PickUpItem>()) return;
        // we collided with a valid pickup item
        // so let that item know it's been 'Picked up' by this game object
        PickUpItem item = other.gameObject.GetComponent<PickUpItem>();
        item.OnPickedUp(gameObject);
    }
}
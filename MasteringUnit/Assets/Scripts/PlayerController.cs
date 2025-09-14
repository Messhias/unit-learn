using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] [Tooltip("The bullet projectile prefab to fire.")]
    private GameObject _bulletToSpawn;

    [SerializeField] [Tooltip("The direction of that the Player is facing.")]
    private Vector3 _currentFacing = new(1, 0, 0);

    // acceleration applied when directional input is received.
    [SerializeField] [Tooltip("How much acceleration is applied to this object when direction input is received.")]
    private float movementAcceleration = 2;

    // the maximum velocity of this object.
    [SerializeField] [Tooltip("The maximum velocity of this object - keeps the player from moving too fast.")]
    private float movementVelocityMax = 2;

    [SerializeField] [Tooltip("Deceleration when no direction input is received.")]
    private float movementDeceleration = 0.1f;

    [FormerlySerializedAs("_jumpVelocity")] [SerializeField] [Tooltip("Upwards force applied when jum kehy is pressed")]
    private float jumpVelocity = 20;

    [FormerlySerializedAs("_extraGravity")] [SerializeField] [Tooltip("Additional gravitational pull")]
    private float extraGravity = 20;
    
    [SerializeField, Tooltip("Are we on the ground?")]
    private bool _isGrounded;

    // the rigid body physics component of this object
    // since we'll be accessing it a lot, we'll store it as member.
    private Rigidbody _rigidbody;
    private Collider _myCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _myCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    private void Update()
    {
        // get the current speed from the rigid body physics component.
        // grabbing this ensures we retain the gravity speed.
        var currentSpeed = _rigidbody.linearVelocity;

        currentSpeed = PlayerMove(ref currentSpeed);

        currentSpeed = PlayerJump(ref currentSpeed);

        AdjustPlayerFriction(ref currentSpeed);

        // fire the weapon?
        FireWeapon();

        // apply max speed
        currentSpeed.x = Mathf.Clamp(currentSpeed.x, movementVelocityMax * -1, movementVelocityMax);
        currentSpeed.z = Mathf.Clamp(currentSpeed.z, movementVelocityMax * -1, movementVelocityMax);

        _rigidbody.linearVelocity = currentSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log($"{other.name} entered");
        // did we collide with the PickUpItem?
        if (!other.gameObject.GetComponent<PickUpItem>()) return;
        // we collided with a valid pickup item
        // so let that item know it's been 'Picked up' by this game object
        var item = other.gameObject.GetComponent<PickUpItem>();
        item.OnPickedUp(gameObject);
    }

    private void FireWeapon()
    {
        if (!Input.GetKeyDown(KeyCode.Return)) return;

        var dir = new Vector3(_currentFacing.x, 0f, _currentFacing.z).normalized;
        if (dir.sqrMagnitude <= Mathf.Epsilon) return;

        var newBullet = Instantiate(
            _bulletToSpawn,
            transform.position,
            Quaternion.LookRotation(dir, Vector3.up)
        );

        var bullet = newBullet.GetComponent<Bullet>();
        bullet?.SetDirection(dir);
    }

    private void AdjustPlayerFriction(ref Vector3 currentSpeed)
    {
        // if both left and right keys are simultaneously pressed (or not pressed), apply friction
        if (Input.GetKey(KeyCode.LeftArrow) == Input.GetKey(KeyCode.RightArrow))
            currentSpeed.x -= movementDeceleration * currentSpeed.x;

        if (Input.GetKey(KeyCode.UpArrow) == Input.GetKey(KeyCode.DownArrow))
            currentSpeed.z -= movementDeceleration * currentSpeed.z;
    }

    private Vector3 PlayerJump(ref Vector3 currentSpeed)
    {
        // does player want to jump?
        if (Input.GetKeyDown(KeyCode.Space) && CalcIsGround() && Mathf.Abs(currentSpeed.y) < 1)
            currentSpeed.y += jumpVelocity;
        else
            currentSpeed.y -= extraGravity * Time.deltaTime;

        return currentSpeed;
    }

    private bool CalcIsGround()
    {
        float offset = 0.1f;
        Vector3 position = _myCollider.bounds.center;
        position.y = _myCollider.bounds.min.y - offset;

        _isGrounded = Physics.CheckSphere(position, offset);

        return _isGrounded;
    }

    private Vector3 PlayerMove(ref Vector3 currentSpeed)
    {
        // Check to see if any of the keyboard arrows are being pressed
        // if so, adjust the speed of the player
        // also store the facing based on the keys being pressed
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentSpeed.x += movementAcceleration * Time.deltaTime;
            _currentFacing.x = 1;
            _currentFacing.z = 0;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentSpeed.x -= movementAcceleration * Time.deltaTime;
            _currentFacing.x = -1;
            _currentFacing.z = 0;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentSpeed.z += movementAcceleration * Time.deltaTime;
            _currentFacing.x = 0;
            _currentFacing.z = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentSpeed.z -= movementAcceleration * Time.deltaTime;
            _currentFacing.x = 0;
            _currentFacing.z = -1;
        }

        return currentSpeed;
    }
}
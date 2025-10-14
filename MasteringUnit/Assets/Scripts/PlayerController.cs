using Base;
using Contracts;
using Exceptions;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour, IPlayerController
{
    #region *** Editor components ***
    
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

    [SerializeField, Tooltip("The player's equipped weapon.")]
    private WeaponBase _weaponEquipped;
    
    private IWeapon Weapon
    {
        get => _weaponEquipped;
        set 
        {
            if (value is WeaponBase weaponBase) 
            {
                _weaponEquipped = weaponBase;
            }
            else
            {
                throw new InvalidWeaponException($"The {value} is not a valid weapon! It is {value.GetType()}");
            }
        }
    }

    #endregion

    #region *** private class members ***
    
    // the rigid body physics component of this object
    // since we'll be accessing it a lot, we'll store it as member.
    private Rigidbody _rigidbody;
    private Collider _myCollider;
    private GameObject _weaponAllocator;
    
    // store whether input was received this frame
    private bool _moveInput;
    
    // the animator controller for this object.
    private Animator _animator;
    
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _myCollider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
        _weaponAllocator = GameObject.Find("WEAPON_LOC");

        if (_animator)
        {
            _animator.Play("Idle");
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // pause movement if we've recently attacked
        if (Weapon is not null && Weapon.IsMovementPaused())
        {
            _rigidbody.linearVelocity = Vector3.zero;
            return;
        }
        
        // get the current speed from the rigid body physics component.
        // grabbing this ensures we retain the gravity speed.
        var currentSpeed = _rigidbody.linearVelocity;

        // reset move input.
        _moveInput = false;
        
        currentSpeed = PlayerMove(ref currentSpeed);

        currentSpeed = PlayerJump(ref currentSpeed);

        AdjustPlayerFriction(ref currentSpeed);

        // fire the weapon?
        FireWeapon();

        // apply max speed
        currentSpeed.x = Mathf.Clamp(currentSpeed.x, movementVelocityMax * -1, movementVelocityMax);
        currentSpeed.z = Mathf.Clamp(currentSpeed.z, movementVelocityMax * -1, movementVelocityMax);

        _rigidbody.linearVelocity = currentSpeed;
        
        transform.LookAt(transform.position + new Vector3(-_currentFacing.x, 0f,  -_currentFacing.z));

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        if (!_animator)
        {
            return;
        }

        if (_moveInput)
            _animator.Play("Run");
        else
        {
            _animator.Play("Idle");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherGameObject = other.gameObject;

        PickUpItem(otherGameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        var otherGameObject = other.gameObject;
        
        PickUpItem(otherGameObject);
    }

    private void PickUpItem(GameObject otherGameObject)
    {
        // did we collide with the PickUpItem?
        if (!otherGameObject.GetComponent<PickUpItem>()) return;
        
        // avoid the player equip "same weapon" again and replace it randomly
        // when pass through other weapon.
        if (Weapon != null)
        {
            var objectName = otherGameObject.name;
            if (objectName.Contains("Sword") || objectName.Contains("Blaster"))
            {
                return;
            }
        }
        
        // we collided with a valid pickup item
        // so let that item know it's been 'Picked up' by this game object
        IPickUpItem item = otherGameObject.GetComponent<PickUpItem>();
        item.OnPickedUp(gameObject);
    }
    
    private void FireWeapon()
    {
        if (Weapon is null)
        {
            return;
        }
        if (!Input.GetKeyDown(KeyCode.Return)) return;


        var direction = new Vector3(_currentFacing.x, 0f, _currentFacing.z).normalized;
        if (direction.sqrMagnitude <= Mathf.Epsilon) return;

        Weapon.OnAttack(direction);

        if (string.IsNullOrEmpty(Weapon.AttackAnimation)) return;
        
        _animator.Play(Weapon.AttackAnimation);
    }

    private void AdjustPlayerFriction(ref Vector3 currentSpeed)
    {
        // if both left and right keys are simultaneously pressed (or not pressed), apply friction
        if (Input.GetKey(KeyCode.LeftArrow) == Input.GetKey(KeyCode.RightArrow))
        {
            currentSpeed.x -= movementDeceleration * currentSpeed.x;
        }

        if (Input.GetKey(KeyCode.UpArrow) == Input.GetKey(KeyCode.DownArrow))
        {
            currentSpeed.z -= movementDeceleration * currentSpeed.z;
        }
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
        var offset = 0.1f;
        var position = _myCollider.bounds.center;
        position.y = _myCollider.bounds.min.y - offset;

        _isGrounded = Physics.CheckSphere(position, offset);

        return _isGrounded;
    }

    private Vector3 PlayerMove(ref Vector3 currentSpeed)
    {
        // Check to see if any of the keyboard arrows are being pressed
        // if so, adjust the speed of the player
        // also store the facing based on the keys being pressed
        
        var right = Input.GetKey(KeyCode.RightArrow) ||  Input.GetKey(KeyCode.D);
        var left = Input.GetKey(KeyCode.LeftArrow) ||  Input.GetKey(KeyCode.A);
        var up =  Input.GetKey(KeyCode.UpArrow) ||  Input.GetKey(KeyCode.W);
        var down = Input.GetKey(KeyCode.DownArrow) ||  Input.GetKey(KeyCode.S);
        
        if (right)
        {
            _moveInput = true;
            currentSpeed.x += movementAcceleration * Time.deltaTime;
            _currentFacing.x = 1;
            _currentFacing.z = 0;
        }

        if (left)
        {
            _moveInput = true;
            currentSpeed.x -= movementAcceleration * Time.deltaTime;
            _currentFacing.x = -1;
            _currentFacing.z = 0;
        }

        if (up)
        {
            _moveInput = true;
            currentSpeed.z += movementAcceleration * Time.deltaTime;
            _currentFacing.x = 0;
            _currentFacing.z = 1;
        }

        if (!down) return currentSpeed;
        
        _moveInput = true;
        currentSpeed.z -= movementAcceleration * Time.deltaTime;
        _currentFacing.x = 0;
        _currentFacing.z = -1;


        return currentSpeed;
    }

    #region Weapons
    
    public void EquipWeapon(IWeapon weapon)
    {
        Weapon = weapon;
        weapon.SetAttachmentParent(_weaponAllocator);
    }
    
    #endregion
}
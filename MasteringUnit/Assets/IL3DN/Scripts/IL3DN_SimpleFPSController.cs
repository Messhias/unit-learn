using UnityEngine.Serialization;

namespace IL3DN
{
    using UnityEngine;
    using Random = UnityEngine.Random;
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(AudioSource))]
    
    ///A simplified version of the FPSController from standard assets 
    public class IL3DnSimpleFPSController : MonoBehaviour
    {
        [FormerlySerializedAs("m_IsWalking")] [SerializeField] private bool mIsWalking = false;
        [FormerlySerializedAs("m_WalkSpeed")] [SerializeField] private float mWalkSpeed = 2;
        [FormerlySerializedAs("m_RunSpeed")] [SerializeField] private float mRunSpeed = 5;
        [FormerlySerializedAs("m_RunstepLenghten")] [SerializeField] [Range(0f, 1f)] private float mRunstepLenghten = 0.7f;
        [FormerlySerializedAs("m_JumpSpeed")] [SerializeField] private float mJumpSpeed = 5;
        [FormerlySerializedAs("m_StickToGroundForce")] [SerializeField] private float mStickToGroundForce = 10;
        [FormerlySerializedAs("m_GravityMultiplier")] [SerializeField] private float mGravityMultiplier = 2;
        [FormerlySerializedAs("m_MouseLook")] [SerializeField] private IL3DnSimpleMouseLook mMouseLook = default;
        [FormerlySerializedAs("m_StepInterval")] [SerializeField] private float mStepInterval = 2;
        [FormerlySerializedAs("m_FootstepSounds")] [SerializeField] private AudioClip[] mFootstepSounds = default;    // an array of footstep sounds that will be randomly selected from.
        [FormerlySerializedAs("m_JumpSound")] [SerializeField] private AudioClip mJumpSound = default;           // the sound played when character leaves the ground.
        [FormerlySerializedAs("m_LandSound")] [SerializeField] private AudioClip mLandSound = default;           // the sound played when character touches back on ground.

        private Camera _mCamera;
        private bool _mJump;
        private float _mYRotation;
        private Vector2 _mInput;
        private Vector3 _mMoveDir = Vector3.zero;
        private CharacterController _mCharacterController;
        private CollisionFlags _mCollisionFlags;
        private bool _mPreviouslyGrounded;
        private float _mStepCycle;
        private float _mNextStep;
        private bool _mJumping;
        private AudioSource _mAudioSource;
        private AudioClip[] _footStepsOverride;
        private AudioClip _jumpSoundOverride;
        private AudioClip _landSoundOverride;
        private bool _isInSpecialSurface;

        /// <summary>
        /// Initialize the controller
        /// </summary>
        private void Start()
        {
            _mCharacterController = GetComponent<CharacterController>();
            _mCamera = Camera.main;
            _mStepCycle = 0f;
            _mNextStep = _mStepCycle / 2f;
            _mJumping = false;
            _mAudioSource = GetComponent<AudioSource>();
            mMouseLook.Init(transform, _mCamera.transform);
        }

        private void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!_mJump)
            {
                _mJump = Input.GetButtonDown("Jump");
            }

            if (!_mPreviouslyGrounded && _mCharacterController.isGrounded)
            {
                PlayLandingSound();
                _mMoveDir.y = 0f;
                _mJumping = false;
            }
            if (!_mCharacterController.isGrounded && !_mJumping && _mPreviouslyGrounded)
            {
                _mMoveDir.y = 0f;
            }

            _mPreviouslyGrounded = _mCharacterController.isGrounded;
        }

        /// <summary>
        /// Plays a sound when Player touches the ground for the first time
        /// </summary>
        private void PlayLandingSound()
        {
            if (_isInSpecialSurface)
            {
                _mAudioSource.clip = _landSoundOverride;
            }
            else
            {
                _mAudioSource.clip = mLandSound;
            }
            _mAudioSource.Play();
            _mNextStep = _mStepCycle + .5f;
        }

        /// <summary>
        /// Move the Player
        /// </summary>
        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            var desiredMove = transform.forward * _mInput.y + transform.right * _mInput.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, _mCharacterController.radius, Vector3.down, out hitInfo,
                               _mCharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            _mMoveDir.x = desiredMove.x * speed;
            _mMoveDir.z = desiredMove.z * speed;


            if (_mCharacterController.isGrounded)
            {
                _mMoveDir.y = -mStickToGroundForce;

                if (_mJump)
                {
                    _mMoveDir.y = mJumpSpeed;
                    PlayJumpSound();
                    _mJump = false;
                    _mJumping = true;
                }
            }
            else
            {
                _mMoveDir += Physics.gravity * mGravityMultiplier * Time.fixedDeltaTime;
            }
            _mCollisionFlags = _mCharacterController.Move(_mMoveDir * Time.fixedDeltaTime);

            ProgressStepCycle(speed);

            mMouseLook.UpdateCursorLock();
        }

        /// <summary>
        /// Plays a jump sound
        /// </summary>
        private void PlayJumpSound()
        {
            if (_isInSpecialSurface)
            {
                _mAudioSource.clip = _jumpSoundOverride;
            }
            else
            {
                _mAudioSource.clip = mJumpSound;
            }
            _mAudioSource.Play();
        }

        /// <summary>
        /// Play foot steps sound based on time and velocity
        /// </summary>
        /// <param name="speed"></param>
        private void ProgressStepCycle(float speed)
        {
            if (_mCharacterController.velocity.sqrMagnitude > 0 && (_mInput.x != 0 || _mInput.y != 0))
            {
                _mStepCycle += (_mCharacterController.velocity.magnitude + (speed * (mIsWalking ? 1f : mRunstepLenghten))) *
                             Time.fixedDeltaTime;
            }

            if (!(_mStepCycle > _mNextStep))
            {
                return;
            }

            _mNextStep = _mStepCycle + mStepInterval;

            PlayFootStepAudio();
        }

        /// <summary>
        /// Plays a random sound for a footstep 
        /// </summary>
        private void PlayFootStepAudio()
        {
            if (!_mCharacterController.isGrounded)
            {
                return;
            }
            if (!_isInSpecialSurface)
            {
                // pick & play a random footstep sound from the array,
                // excluding sound at index 0
                var n = Random.Range(1, mFootstepSounds.Length);
                _mAudioSource.clip = mFootstepSounds[n];
                _mAudioSource.PlayOneShot(_mAudioSource.clip);
                // move picked sound to index 0 so it's not picked next time
                mFootstepSounds[n] = mFootstepSounds[0];
                mFootstepSounds[0] = _mAudioSource.clip;

            }
            else
            {
                var n = Random.Range(1, _footStepsOverride.Length);
                if (n >= _footStepsOverride.Length)
                {
                    n = 0;
                }
                _mAudioSource.clip = _footStepsOverride[n];
                _mAudioSource.PlayOneShot(_mAudioSource.clip);
                _footStepsOverride[n] = _footStepsOverride[0];
                _footStepsOverride[0] = _mAudioSource.clip;
            }
        }

        /// <summary>
        /// Reads user input
        /// </summary>
        /// <param name="speed"></param>
        private void GetInput(out float speed)
        {
            // Read input
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var waswalking = mIsWalking;
#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            mIsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = mIsWalking ? mWalkSpeed : mRunSpeed;
            _mInput = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (_mInput.sqrMagnitude > 1)
            {
                _mInput.Normalize();
            }
        }

        /// <summary>
        /// Moves camera based on player position
        /// </summary>
        private void RotateView()
        {
            mMouseLook.LookRotation(transform, _mCamera.transform);
        }

        /// <summary>
        /// Used to determine if a player is in a special area to override  the sounds
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            var soundScript = other.GetComponent<IL3DnChangeWalkingSound>();
            if (soundScript != null)
            {
                _footStepsOverride = soundScript.footStepsOverride;
                _jumpSoundOverride = soundScript.jumpSound;
                _landSoundOverride = soundScript.landSound;
                _isInSpecialSurface = true;
            }
        }

        /// <summary>
        /// Player exits the special area
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            _isInSpecialSurface = false;
        }
    }
}


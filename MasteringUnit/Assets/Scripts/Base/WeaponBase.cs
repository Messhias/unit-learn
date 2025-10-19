using Contracts;
using UnityEngine;
using UnityEngine.Serialization;

public enum WeaponType
{
    Sword,
    FireArm
}

namespace Base
{
    public abstract class WeaponBase : MonoBehaviour, IWeapon
    {
        
        #region Editor Fields
        
        public AudioClip attackAudioClip;

        [FormerlySerializedAs("_attackAnimation")] [SerializeField, Tooltip("Animation to play when attacking")]
        private string attackAnimation = "SwingSowrd_01";

        [SerializeField, Tooltip("Identify the weapon type it is")]
        protected WeaponType weaponType;

        #endregion
        
        public string AttackAnimation
        {
            get => attackAnimation;
            set => attackAnimation = value;
        }

        #region *** Protected Properties ***

        protected float PauseMovementMax
        {
            get => pauseMovementMax;
            set => pauseMovementMax = value;
        }

        #endregion


        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }


        // Métodos que podem ser compartilhados
        private void Update()
        {
            if (PauseMovementTimer > 0f)
            {
                PauseMovementTimer -= Time.deltaTime;
                // return;
            }

            if (!_attachmentParent) return;


            var attachmentTransform = _attachmentParent.transform;
            transform.position = attachmentTransform.position;
            transform.localEulerAngles = attachmentTransform.localEulerAngles;
            ResetWeaponBodyConstraints();
        }

        private void OnTriggerEnter(Collider other)
        {
            var target = other.gameObject;

            // If we attack spikes.
            if (target.name.Contains("EnemyObj_Spikes"))
            {
                var vfxHandler = target.GetComponent<VFXHandler>();
                vfxHandler?.SpawnExplosion();
                Destroy(target);
            }
        }

        public void SetAttachmentParent(GameObject attachment)
        {
            _attachmentParent = attachment;
        }

        public bool IsMovementPaused()
        {
            return PauseMovementTimer > 0f;
        }

        public abstract void OnAttack(Vector3 facing);

        private void ResetWeaponBodyConstraints()
        {
            Rigidbody.constraints = InitialConstraints;
        }

        #region *** Editor config ***

        [FormerlySerializedAs("_pauseMovementMax")] [SerializeField] [Tooltip("Pause movement after an attack?")]
        private float pauseMovementMax = 1.0f;

        [FormerlySerializedAs("_bulletToSpawn")] [SerializeField] [Tooltip("The bullet projectile to fire.")]
        internal GameObject bulletToSpawn;

        #endregion

        #region *** Private Properties ***

        private GameObject _attachmentParent;

        internal RigidbodyConstraints InitialConstraints;
        internal Rigidbody Rigidbody;
        internal float PauseMovementTimer { get; set; }

        #endregion
    }
}
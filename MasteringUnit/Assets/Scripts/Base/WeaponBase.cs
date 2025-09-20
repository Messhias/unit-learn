using System;
using Contracts;
using UnityEngine;

namespace Base
{
    public abstract class WeaponBase : MonoBehaviour, IWeapon
    {
        #region *** Editor config ***
        
        [SerializeField, Tooltip("Pause movement after an attack?")]
        private float _pauseMovementMax = 1.0f;
        
        #endregion 
        
        #region *** Private Properties ***

        private GameObject _attachmentParent;
        
        internal RigidbodyConstraints _initialConstraints;
        internal Rigidbody _rigidbody;
        internal float PauseMovementTimer { get; set; }
        
        #endregion
        
        #region *** Protected Properties ***
            protected float PauseMovementMax
            {
                get => _pauseMovementMax;
                set => _pauseMovementMax = value;
            }



            #endregion


        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }


        // Métodos que podem ser compartilhados
        private void Update()
        {
            if (PauseMovementTimer > 0f)
            {
                PauseMovementTimer -= Time.deltaTime;
                return;
            }

            if (!_attachmentParent) return;
            
            
            Transform attachmentTransform = _attachmentParent.transform;
            transform.position = attachmentTransform.position;
            transform.localEulerAngles = attachmentTransform.localEulerAngles;
            ResetWeaponBodyConstraints();
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
            _rigidbody.constraints = _initialConstraints;
        }
    }
}
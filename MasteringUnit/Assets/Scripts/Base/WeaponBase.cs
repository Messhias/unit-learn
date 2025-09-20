using Contracts;
using UnityEngine;

namespace Base
{
    public abstract class WeaponBase : MonoBehaviour, IWeapon
    {
        [SerializeField, Tooltip("Pause movement after an attack?")]
        protected float _pauseMovementMax = 1.0f;
        protected float _pauseMovementTimer;
    
        protected GameObject _attachmentParent;
    
        // Métodos que podem ser compartilhados
        protected virtual void Update()
        {
            if (_pauseMovementTimer > 0f)
            {
                _pauseMovementTimer -= Time.deltaTime;
                return;
            }

            if (_attachmentParent)
            {
                Transform tr = _attachmentParent.transform;
                transform.position = tr.position;
                transform.localEulerAngles = tr.localEulerAngles;
            }
        }
    
        public void SetAttachmentParent(GameObject attachment)
        {
            _attachmentParent = attachment;
        }

        public bool IsMovementPaused()
        {
            return _pauseMovementTimer > 0f;
        }

        public abstract void OnAttack(Vector3 facing);
    }
}
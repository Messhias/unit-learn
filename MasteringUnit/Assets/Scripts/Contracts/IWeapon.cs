using UnityEngine;

namespace Contracts
{
    public interface IWeapon
    {
        void SetAttachmentParent(GameObject attachment);
        bool IsMovementPaused();
        void OnAttack(Vector3 facing);
    }
}
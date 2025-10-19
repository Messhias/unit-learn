using UnityEngine;

namespace Contracts
{
    public interface IWeapon
    {
        string AttackAnimation { get; set; }
        void SetAttachmentParent(GameObject attachment);
        bool IsMovementPaused();
        void OnAttack(Vector3 facing);
    }
}
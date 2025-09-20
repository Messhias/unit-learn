using Base;
using UnityEngine;

public class Weapon : WeaponBase
{
    public override void OnAttack(Vector3 facing)
    {
        transform.position += facing;
        transform.Rotate(new Vector3(45f, -100f, 60f));
        PauseMovementTimer = PauseMovementMax;

        _initialConstraints = _rigidbody.constraints;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    
}
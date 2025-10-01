using System;
using Base;
using Contracts;
using UnityEngine;

public class Weapon : WeaponBase
{
    public override void OnAttack(Vector3 facing)
    {
        SpawnedSoundFX.Spawn(transform.position, attackAudioClip);

        switch (weaponType)
        {
            case WeaponType.FireArm:
                FireArmAttack(facing);
                break;
            case WeaponType.Sword:
                SwordAttack();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SwordAttack()
    {
        PauseMovementTimer = PauseMovementMax;

        InitialConstraints = Rigidbody.constraints;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FireArmAttack(Vector3 facing)
    {
        if (bulletToSpawn is not { } bulletTo) return;
            
        var newBullet = Instantiate(
            bulletTo,
            transform.position,
            Quaternion.identity
        );

        IBullet bullet = newBullet.GetComponent<Bullet>();

        bullet?.SetDirection(new Vector3(facing.x, 0f, facing.z));
    }
}
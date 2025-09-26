using Base;
using Contracts;
using UnityEngine;

public class Weapon : WeaponBase
{
    public override void OnAttack(Vector3 facing)
    {
        if (bulletToSpawn)
        {
            var newBullet = Instantiate(
                bulletToSpawn,
                transform.position,
                Quaternion.identity
            );

            IBullet bullet = newBullet.GetComponent<Bullet>();

            bullet?.SetDirection(new Vector3(facing.x, 0f, facing.z));

            return;
        }
        
        PauseMovementTimer = PauseMovementMax;

        InitialConstraints = Rigidbody.constraints;
        Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
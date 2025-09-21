using Base;
using Contracts;
using UnityEngine;

public class Weapon : WeaponBase
{
    public override void OnAttack(Vector3 facing)
    {
        if (_bulletToSpawn)
        {
            var newBullet = Instantiate(
                _bulletToSpawn,
                transform.position,
                Quaternion.identity
            );
            
            IBullet bullet =  newBullet.GetComponent<Bullet>();

            bullet?.SetDirection(new Vector3(facing.x, 0f, facing.z));

            return;
        }
        
        
        transform.position += facing;
        transform.Rotate(new Vector3(45f, -100f, 60f));
        PauseMovementTimer = PauseMovementMax;

        _initialConstraints = _rigidbody.constraints;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }
    
}
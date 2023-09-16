using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using TopDownCharacter2D.Attacks;
using TopDownCharacter2D.Attacks.Range;

public class ShootTarget : CharacterAction
{
    public SharedVector2 directionToTarget;
    public AttackConfig attackConfig;
    
    [SerializeField]
    private Transform projectileSpawnPosition;
    
    [SerializeField]
    private bool projectileSizeModifyRecoil = true;
    
    [SerializeField]
    private float recoilStrength = 1f;
    
    public override TaskStatus OnUpdate()
    {
        characterController.OnAttackEvent.Invoke(attackConfig);
        Shoot(attackConfig);
        return TaskStatus.Success;
    }
    
    private void Shoot(AttackConfig attackConfig)
    {
        RangedAttackConfig rangedAttackConfig = (RangedAttackConfig) attackConfig;
        float projectilesAngleSpace = rangedAttackConfig.multipleProjectilesAngle;
        float minAngle = -(rangedAttackConfig.numberOfProjectilesPerShot / 2f) * projectilesAngleSpace +
                         0.5f * rangedAttackConfig.multipleProjectilesAngle;

        for (int i = 0; i < rangedAttackConfig.numberOfProjectilesPerShot; i++)
        {
            float angle = minAngle + projectilesAngleSpace * i;
            float randomSpread = Random.Range(-rangedAttackConfig.spread, rangedAttackConfig.spread);
            angle += randomSpread;
            CreateProjectile(rangedAttackConfig, angle);
        }

        if (body != null)
        {
            AddRecoil(rangedAttackConfig);
        }
    }
    
    /// <summary>
    ///     Creates a projectile matching the projectile's parameters
    /// </summary>
    /// <param name="rangedAttackConfig"> the configuration of the projectile to shoot </param>
    /// <param name="angle"> Modification to the direction of the shot</param>
    private void CreateProjectile(RangedAttackConfig rangedAttackConfig, float angle)
    {
        ProjectileManager.instance.ShootBullet(characterController.projectileSpawnPosition.position, RotateVector2(directionToTarget.Value.normalized, angle),
            rangedAttackConfig);
    }

    /// <summary>
    ///     Rotates a Vector2 by a set amount of degrees
    /// </summary>
    /// <param name="v"> The vector to rotate </param>
    /// <param name="degrees"> The angle in degree </param>
    /// <returns></returns>
    private static Vector2 RotateVector2(Vector2 v, float degrees)
    {
        return Quaternion.Euler(0, 0, degrees) * v;
    }

    /// <summary>
    ///     Adds a recoil matching the size of the projectile
    /// </summary>
    /// <param name="rangedAttackConfig"> the configuration of the projectile shot </param>
    private void AddRecoil(RangedAttackConfig rangedAttackConfig)
    {
        if (projectileSizeModifyRecoil)
        {
            body.AddForce(-(directionToTarget.Value * (rangedAttackConfig.size * recoilStrength * 100f)), ForceMode2D.Impulse);
        }
        else
        {
            body.AddForce(-(directionToTarget.Value * (recoilStrength * 100f)), ForceMode2D.Impulse);
        }
    }
}

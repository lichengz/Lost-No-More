using System;
using TopDownCharacter2D.Attacks;
using TopDownCharacter2D.Attacks.Range;
using TopDownCharacter2D.Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TopDownCharacter2D
{
    /// <summary>
    ///     This class contains the logic for a shooting behaviour in a 2D context with a topdown view
    /// </summary>
    [RequireComponent(typeof(TopDownCharacterController))]
    public class TopDownShooting : MonoBehaviour
    {
        [Header("Parameters")] [SerializeField] [Range(0.0f, 2f)] [Tooltip("The strength of the recoil after a shot")]
        private float recoilStrength = 1f;

        [SerializeField] [Tooltip("Define if the recoil is proportional to the size of the projectile")]
        private bool projectileSizeModifyRecoil = true;


        [SerializeField] [Tooltip("The start point of the projectiles")]
        private Transform projectileSpawnPosition => _controller.projectileSpawnPosition;

        private Vector2 _aimDirection = Vector2.right;
        private ProjectileManager projectileManager;
        private TopDownCharacterController _controller;
        private Rigidbody2D _rb;
        private RangedAttackConfig rangedAttackConfig;
        private bool recoil;

        [SerializeField]
        private EquipRangeWeaponEventChannelSO equipRangeWeapon;
        [SerializeField] 
        private VoidEventChannelSO unEquipWeapons;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _controller = GetComponent<TopDownCharacterController>();
            projectileManager = ProjectileManager.instance;
        }

        private void OnEnable()
        {
            equipRangeWeapon.OnEventRaised += EquipRangeWeapon;
            unEquipWeapons.OnEventRaised += UnEquipRangeWeapon;
        }

        private void OnDisable()
        {
            equipRangeWeapon.OnEventRaised -= EquipRangeWeapon;
            unEquipWeapons.OnEventRaised -= UnEquipRangeWeapon;
        }

        private void FixedUpdate()
        {
            if (recoil)
            {
                if (_rb != null)
                {
                    AddRecoil(rangedAttackConfig);
                }
                recoil = false;
            }
        }

        public void EquipRangeWeapon(RangedAttackConfig config)
        {
            _controller.OnAttackEvent.AddListener(OnShoot);
            _controller.LookEvent.AddListener(OnAim);
        }
        
        public void UnEquipRangeWeapon()
        {
            _controller.OnAttackEvent.RemoveListener(OnShoot);
            _controller.LookEvent.RemoveListener(OnAim);
        }

        /// <summary>
        ///     To call when the aim changes direction
        /// </summary>
        /// <param name="newAimDirection"> The new aim direction </param>
        public void OnAim(Vector2 newAimDirection)
        {
            _aimDirection = newAimDirection.normalized;
        }

        /// <summary>
        ///     To call when the player tries to shoot
        /// </summary>
        /// <param name="attackConfig"> The stats of the projectile to shoot </param>
        public void OnShoot(AttackConfig attackConfig)
        {
            rangedAttackConfig = (RangedAttackConfig) attackConfig;
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

            recoil = true;
        }

        /// <summary>
        ///     Creates a projectile matching the projectile's parameters
        /// </summary>
        /// <param name="rangedAttackConfig"> the configuration of the projectile to shoot </param>
        /// <param name="angle"> Modification to the direction of the shot</param>
        private void CreateProjectile(RangedAttackConfig rangedAttackConfig, float angle)
        {
            projectileManager.ShootBullet(projectileSpawnPosition.position, RotateVector2(_aimDirection.normalized, angle),
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
            _rb.velocity = Vector2.zero;
            if (projectileSizeModifyRecoil)
            {
                _rb.AddForce(-(_aimDirection * (rangedAttackConfig.size * recoilStrength * 100f)), ForceMode2D.Impulse);
            }
            else
            {
                _rb.AddForce(-(_aimDirection * (recoilStrength * 100f)), ForceMode2D.Impulse);
            }
        }
    }
}
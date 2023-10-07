using System;
using TopDownCharacter2D.Attacks;
using TopDownCharacter2D.Attacks.Melee;
using TopDownCharacter2D.Controllers;
using UnityEngine;

namespace TopDownCharacter2D
{
    /// <summary>
    ///     Handles the logic behind a close combat attack
    /// </summary>
    [RequireComponent(typeof(TopDownCharacterController))]
    public class TopDownMelee : MonoBehaviour
    {
        [Header("Parameters")] [SerializeField] [Range(0.0f, 2f)] [Tooltip("The strength of the rush after an attack")]
        private float rushStrength = 1f;
        
        [SerializeField] private GameObject attackObject;

        private Vector2 _attackDirection;

        private Rigidbody2D _rb;
        private TopDownCharacterController _controller;
        private bool rush;

        [SerializeField] private EquipMeleeWeaponEventChannelSO equipMeleeWeapon;
        [SerializeField] private VoidEventChannelSO unEquipWeapons;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _controller = GetComponent<TopDownCharacterController>();
            if (!transform.CompareTag("Player"))
            {
                // TODO
                _controller.LookEvent.AddListener(Rotate);
            }
        }

        private void OnEnable()
        {
            if (equipMeleeWeapon != null)
                equipMeleeWeapon.OnEventRaised += EquipMeleeWeapon;
            if (unEquipWeapons != null)
                unEquipWeapons.OnEventRaised += UnEquipMeleeWeapon;
        }

        private void OnDisable()
        {
            if (equipMeleeWeapon != null)
                equipMeleeWeapon.OnEventRaised -= EquipMeleeWeapon;
            if (unEquipWeapons != null)
                unEquipWeapons.OnEventRaised -= UnEquipMeleeWeapon;
        }

        private void FixedUpdate()
        {
            if (rush)
            {
                AddRush();
                rush = false;
            }
        }

        private void EquipMeleeWeapon(MeleeAttackConfig config)
        {
            _controller.OnAttackEvent.AddListener(Attack);
            _controller.LookEvent.AddListener(Rotate);
        }

        private void UnEquipMeleeWeapon()
        {
            _controller.OnAttackEvent.RemoveListener(Attack);
            _controller.LookEvent.RemoveListener(Rotate);
        }

        private void Attack(AttackConfig config)
        {
            if (!(config is MeleeAttackConfig))
            {
                return;
            }

            InstantiateAttack((MeleeAttackConfig)config);
        }

        private void Rotate(Vector2 rotation)
        {
            _attackDirection = rotation;
        }

        /// <summary>
        ///     Creates an attack object
        /// </summary>
        /// <param name="attackConfig"> The configuration on the melee attack</param>
        private void InstantiateAttack(MeleeAttackConfig attackConfig)
        {
            Transform attackPivot = _controller.projectileSpawnPosition;
            attackPivot.localRotation = Quaternion.identity;
            GameObject obj = Instantiate(attackConfig.projectile, attackPivot.position,
                Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.right, _attackDirection)), attackPivot);
            MeleeAttackController attackController = obj.GetComponent<MeleeAttackController>();
            attackController.InitializeAttack(attackConfig, _controller.weaponRenderer);
            // AddRush();
            rush = true;
        }

        private void AddRush()
        {
            if (_rb == null) return;
            _rb.velocity = Vector2.zero;
            _rb.AddForce(_attackDirection * rushStrength * 100f, ForceMode2D.Impulse);
        }

        public void AttackEvent()
        {
            // TODO 
            InstantiateAttack((MeleeAttackConfig)_controller.Stats.CurrentStats.attackConfig);
        }
    }
}
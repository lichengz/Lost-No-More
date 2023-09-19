using System;
using System.Collections;
using System.Collections.Generic;
using TopDownCharacter2D.Attacks;
using TopDownCharacter2D.Stats;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Aoiti.Pathfinding;
using TopDownCharacter2D.Attacks.Melee;
using TopDownCharacter2D.Attacks.Range;
using Random = UnityEngine.Random;

namespace TopDownCharacter2D.Controllers
{
    /// <summary>
    ///     A basic controller for a character
    /// </summary>
    [RequireComponent(typeof(CharacterStatsHandler))]
    public abstract class TopDownCharacterController : MonoBehaviour
    {
        private Rigidbody2D _rb;

        [Tooltip("The origin point of the arm to aim with")]
        public Transform weaponPivot;

        [HideInInspector] public Transform projectileSpawnPosition;

        [HideInInspector] public SpriteRenderer weaponRenderer;

        public bool isWeaponInited { get; set; }

        [Tooltip("The main renderer of the character")]
        public List<SpriteRenderer> characterRenderers;

        private float _timeSinceLastAttack = float.MaxValue;

        protected bool IsAttacking { get; set; }
        protected CharacterStatsHandler Stats { get; private set; }

        Vector3Int[] directions = new Vector3Int[4]
            { Vector3Int.left, Vector3Int.right, Vector3Int.up, Vector3Int.down };

        public Tilemap tilemap;
        public TileAndMovementCost[] tiles;
        Pathfinder<Vector3Int> pathfinder;

        [System.Serializable]
        public struct TileAndMovementCost
        {
            public Tile tile;
            public bool movable;
            public float movementCost;
        }

        private List<Vector3Int> path;

        public List<Vector3Int> Path
        {
            get { return path; }
        }

        public bool isReachEnd => path.Count == 0;
        public Vector3 velocity => _rb.velocity;
        public bool jumpEnabled => false;
        public bool isGrounded => true;
        public float stepOffset => 2;
        public float slopeLimit => 0.5f;

        public float DistanceFunc(Vector3Int a, Vector3Int b)
        {
            return (a - b).sqrMagnitude;
        }

        public float DistanceFunc(Vector3 a, Vector3 b)
        {
            return (a - b).sqrMagnitude;
        }


        public Dictionary<Vector3Int, float> connectionsAndCosts(Vector3Int a)
        {
            Dictionary<Vector3Int, float> result = new Dictionary<Vector3Int, float>();
            foreach (Vector3Int dir in directions)
            {
                foreach (TileAndMovementCost tmc in tiles)
                {
                    if (tilemap.GetTile(a + dir) == tmc.tile)
                    {
                        if (tmc.movable) result.Add(a + dir, tmc.movementCost);
                    }
                }
            }

            return result;
        }

        protected virtual void Awake()
        {
            Stats = GetComponent<CharacterStatsHandler>();
            _rb = GetComponent<Rigidbody2D>();
            pathfinder = new Pathfinder<Vector3Int>(DistanceFunc, connectionsAndCosts);
        }

        protected void Start()
        {
            InitializeWeapon();
        }

        protected virtual void Update()
        {
            HandleAttackDelay();
        }

        public void SchedulePath(Vector3 target)
        {
            target.z = 0;
            var currentCellPos = tilemap.WorldToCell(_rb.position);
            pathfinder.GenerateAstarPath(currentCellPos, Vector3Int.FloorToInt(target), out path);
            // StopAllCoroutines();
            // StartCoroutine(MovePathCoroutine());
        }

        IEnumerator MovePathCoroutine()
        {
            while (path.Count > 0)
            {
                Vector2 targetPos = tilemap.CellToWorld(path[0]);
                float distance = DistanceFunc(targetPos, _rb.position);
                while (distance > 0.1f)
                {
                    OnMoveEvent.Invoke((targetPos - _rb.position).normalized);
                    distance = DistanceFunc(targetPos, _rb.position);
                    yield return null;
                }

                if (path.Count > 0) path.RemoveAt(0);
            }
        }

        public void ClearPath()
        {
            path.Clear();
        }

        protected virtual void InitializeWeapon()
        {
            if (transform.CompareTag("Player")) return;
            GameObject weapon = Instantiate(Stats.CurrentStats.attackConfig.weaponPrefab, weaponPivot);
            WeaponController weaponController = weapon.GetComponent<WeaponController>();
            weaponRenderer = weaponController.weaponRenderer;
            projectileSpawnPosition = weaponController.projectileSpawnPosition;
            isWeaponInited = true;
        }

        public WeaponController.WeaponType GetWeaponType()
        {
            if (Stats.CurrentStats.attackConfig is RangedAttackConfig)
            {
                return WeaponController.WeaponType.Range;
            }else if (Stats.CurrentStats.attackConfig is MeleeAttackConfig)
            {
                return WeaponController.WeaponType.Melee;
            }
            return WeaponController.WeaponType.Melee;
        }

        /// <summary>
        ///     Only trigger a attack event when the attack delay is over
        /// </summary>
        private void HandleAttackDelay()
        {
            if (Stats.CurrentStats.attackConfig == null)
            {
                return;
            }

            if (_timeSinceLastAttack <= Stats.CurrentStats.attackConfig.delay)
            {
                _timeSinceLastAttack += Time.deltaTime;
            }

            if (IsAttacking && _timeSinceLastAttack > Stats.CurrentStats.attackConfig.delay)
            {
                _timeSinceLastAttack = 0f;
                onAttackEvent.Invoke(Stats.CurrentStats.attackConfig);
                IsAttacking = false;
            }
        }

        protected void HandleAim(Vector2 look)
        {
            if (!(look.normalized == look))
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(look);
                look = (worldPos - (Vector2)transform.position).normalized;
            }

            if (look.magnitude >= .9f)
            {
                LookEvent.Invoke(look);
            }
        }

        #region Events

        private readonly MoveEvent onMoveNavMeshEvent = new MoveEvent();
        private readonly MoveEvent onMoveEvent = new MoveEvent();
        private readonly AttackEvent onAttackEvent = new AttackEvent();
        private readonly LookEvent onLookEvent = new LookEvent();

        public UnityEvent<Vector2> OnMoveNavMeshEvent => onMoveNavMeshEvent;
        public UnityEvent<Vector2> OnMoveEvent => onMoveEvent;
        public UnityEvent<AttackConfig> OnAttackEvent => onAttackEvent;
        public UnityEvent<Vector2> LookEvent => onLookEvent;

        #endregion
    }
}
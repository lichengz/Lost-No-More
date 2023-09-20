using System;
using System.Collections;
using System.Collections.Generic;
using TopDownCharacter2D.Controllers;
using TopDownCharacter2D.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace TopDownCharacter2D
{
    /// <summary>
    ///     This class contains the logic for movement in a 2D environment with a top down view
    /// </summary>
    [RequireComponent(typeof(CharacterStatsHandler))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(TopDownCharacterController))]
    public class TopDownMovement : MonoBehaviour
    {
        private TopDownCharacterController _controller;

        private Vector2 _movementDirection = Vector2.zero;
        private Rigidbody2D _rb;
        private CharacterStatsHandler _stats;
        private Damageable _damageable;
        private NavMeshAgent _agent;

        [Range(0.01f, 1f)] public float stepTime;

        private void Awake()
        {
            _controller = GetComponent<TopDownCharacterController>();
            _stats = GetComponent<CharacterStatsHandler>();
            _rb = GetComponent<Rigidbody2D>();
            _damageable = GetComponent<Damageable>();
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _controller.OnMoveEvent.AddListener(Move);
            _controller.OnMoveNavMeshEvent.AddListener(MoveNavMesh);
            if (_agent != null)
            {
                _agent.updateRotation = false;
                _agent.updateUpAxis = false;
            }
        }

        void Update()
        {
            if (_agent != null && _agent.isActiveAndEnabled && Input.GetMouseButtonDown(1))
            {
                var target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                target.z = 0;
                _agent.destination = target;
            }
        }

        private void FixedUpdate()
        {
            if (transform.CompareTag("Player") && !_damageable.GetHit)
            {
                ApplyMovement(_movementDirection);
            }
        }

        /// <summary>
        ///     Changes the current direction of the movement
        /// </summary>
        /// <param name="direction"></param>
        private void Move(Vector2 direction)
        {
            _movementDirection = direction;
        }

        private void MoveNavMesh(Vector2 target)
        {
            _agent.destination = target;
        }

        /// <summary>
        ///     Used to apply a given movement vector to the rigidbody of the character
        /// </summary>
        /// <param name="direction"> The direction in which to move the rigidbody </param>
        private void ApplyMovement(Vector2 direction)
        {
            _rb.velocity = direction * _stats.CurrentStats.speed;
        }
    }
}
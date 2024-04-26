

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace TopDownCharacter2D.Controllers
{
    /// <summary>
    ///     A basic controller for an enemy
    /// </summary>
    public class TopDownEnemyController : TopDownCharacterController
    {
        [Tooltip("The tag of the target of this enemy")] [SerializeField]
        private string targetTag = "Player";
        private NavMeshAgent agent;


        protected string TargetTag => targetTag;
        protected Transform ClosestTarget { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            agent = GetComponent<NavMeshAgent>();
            ClosestTarget = FindClosestTarget();
        }

        // private void Update()
        // {
        //     base.Update();
        //     if (characterRenderers != null)
        //     {
        //         foreach (var sprite in characterRenderers)
        //         {
        //             sprite.flipX = agent.destination.x <= transform.position.x;
        //         }
        //     }
        //     // foreach (var sprite in characterRenderers)
        //     // {
        //     //     Debug.Log("!!!" +  sprite.flipX);
        //     // }
        // }

        protected virtual void FixedUpdate()
        {
            ClosestTarget = FindClosestTarget();
        }

        /// <summary>
        ///     Returns the closest valid target
        /// </summary>
        /// <returns> The transform of the closest target</returns>
        private Transform FindClosestTarget()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
            if (targets.Length == 0)
            {
                return null;
            }
            return GameObject.FindGameObjectsWithTag(targetTag)
                .OrderBy(o => Vector3.Distance(o.transform.position, transform.position))
                .First().transform;
        }

        /// <summary>
        ///     Computes and returns the distance to the closest target
        /// </summary>
        /// <returns></returns>
        protected float DistanceToTarget()
        {
            if (ClosestTarget == null) return 0;
            return Vector3.Distance(transform.position, ClosestTarget.transform.position);
        }

        /// <summary>
        ///     Computes and returns the direction toward the closest target
        /// </summary>
        /// <returns></returns>
        protected Vector2 DirectionToTarget()
        {
            if (ClosestTarget == null) return Vector2.zero;
            return (ClosestTarget.transform.position - transform.position).normalized;
        }
    }
}
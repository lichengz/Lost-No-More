using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using TopDownCharacter2D;
using TopDownCharacter2D.Controllers;

namespace TheKiwiCoder
{
    [System.Serializable]
    
    public class MoveTowardsTileMap : ActionNode
    {
        private TopDownCharacterController topDownCharacterController;
        
        [Tooltip("Target Position")] 
        public NodeProperty<Vector2> targetPosition = new NodeProperty<Vector2> { defaultValue = Vector3.zero };
        
        protected override void OnStart()
        {
            topDownCharacterController = context.topDownCharacterController;
            if (topDownCharacterController != null)
            {
                topDownCharacterController.MovePath(targetPosition.Value);
            }
            else
            {
                Debug.LogError("no Movable component found!");
            }
        }
    
        protected override void OnStop() {
        }
    
        protected override State OnUpdate() {
            if (topDownCharacterController.isReachEnd)
            {
                return State.Success;
            }
            else
            {
                return State.Running;
            }
        }
    }
}

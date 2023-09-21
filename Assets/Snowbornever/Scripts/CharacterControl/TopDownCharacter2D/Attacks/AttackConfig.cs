using UnityEngine;

namespace TopDownCharacter2D.Attacks
{
    /// <summary>
    ///     The base class for an attack configuration
    /// </summary>
    public abstract class AttackConfig : ScriptableObject
    {
        [Tooltip("The prefab of the weapon")]
        public GameObject weaponPrefab;
        
        [Tooltip("The projectile of the weapon")]
        public GameObject projectile;
        
        [Tooltip("The scale of the attack")]
        public float size;

        [Tooltip("The time between two attacks")]
        public float delay;
        
        [Tooltip("The damage dealt by an attack")]
        public float power;
        
        [Tooltip("The speed of the attack")]
        public float speed;
        
        [Tooltip("The possible targets for this attack")]
        public LayerMask target;
    }
}
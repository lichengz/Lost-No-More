using BehaviorDesigner.Runtime.Tasks;
using Core.Character;
using Core.Combat;
using UnityEngine;

public class CharacterCondition : Conditional
{
    protected Rigidbody2D body;
    protected Animator animator;
    protected Damageable damageable;
    protected PlayerController player;

    public override void OnAwake()
    {
        body = GetComponent<Rigidbody2D>();
        player = PlayerController.Instance;
        damageable = GetComponent<Damageable>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }
}
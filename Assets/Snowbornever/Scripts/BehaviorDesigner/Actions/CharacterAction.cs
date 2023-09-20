using BehaviorDesigner.Runtime.Tasks;
using Core.Character;
using Core.Combat;
using TopDownCharacter2D.Controllers;
using UnityEngine;
using UnityEngine.AI;

public class CharacterAction : Action
{
    protected LayerMask wallLayerMask = LayerMask.GetMask("interactable");
    protected Rigidbody2D body;
    protected Animator animator;
    protected TopDownCharacterController characterController;
    protected NavMeshAgent agent;
    
    protected readonly int IsWalking = Animator.StringToHash("IsWalking");
    protected readonly int Attack = Animator.StringToHash("Attack");
    protected readonly int IsHurt = Animator.StringToHash("IsHurt");

    public override void OnAwake()
    {
        body = GetComponent<Rigidbody2D>();
        characterController = GetComponent<TopDownCharacterController>();
        animator = gameObject.GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
}
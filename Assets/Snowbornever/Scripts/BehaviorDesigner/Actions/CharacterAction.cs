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

    public override void OnAwake()
    {
        body = GetComponent<Rigidbody2D>();
        characterController = GetComponent<TopDownCharacterController>();
        animator = gameObject.GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
}
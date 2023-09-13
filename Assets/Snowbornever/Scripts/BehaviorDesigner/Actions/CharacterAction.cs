using BehaviorDesigner.Runtime.Tasks;
using Core.Character;
using Core.Combat;
using TopDownCharacter2D.Controllers;
using UnityEngine;

public class CharacterAction : Action
{
    protected LayerMask wallLayerMask = LayerMask.GetMask("interactable");
    protected Rigidbody2D body;
    protected Animator animator;
    protected TopDownCharacterController characterController;

    public override void OnAwake()
    {
        body = GetComponent<Rigidbody2D>();
        characterController = GetComponent<TopDownCharacterController>();
        animator = gameObject.GetComponentInChildren<Animator>();
    }
}
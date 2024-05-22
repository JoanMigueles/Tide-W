using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Troop
{
    [SerializeField] private Animator animator;

    public override void Attack()
    {
        // Play the attack animation
        animator.SetTrigger("Attack");
    }

    public void AttackOnAnimationTrigger()
    {
        base.Attack();
    }
}
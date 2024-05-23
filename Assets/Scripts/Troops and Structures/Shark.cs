using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Troop
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;

    public override void Attack()
    {
        // Play the attack animation
        animator.SetTrigger("Attack");
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void AttackOnAnimationTrigger()
    {
        base.Attack();
        
    }
}
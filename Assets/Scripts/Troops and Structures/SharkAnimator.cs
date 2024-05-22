using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAnimator : MonoBehaviour
{
    public void Attack()
    {
        Shark s = GetComponentInParent<Shark>();
        s.AttackOnAnimationTrigger();
    }
}

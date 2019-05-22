using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimController
{
    public static void AttackHand(Animator animator)
    {
        animator.SetTrigger("AttackHand");
    }

    public static void AttackPalkoi(Animator animator)
    {
        animator.SetTrigger("AttackPalkoi");
    }

    public static void Walk(Animator animator, bool flag)
    {
        animator.SetBool("Walk", flag);
    }

    public static void SpeedWalk(Animator animator, float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public static void MeleeOneHandedWeapon(Animator animator, bool flag)
    {
        animator.SetBool("MeleeWeapon", flag);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationCtrl : MonoBehaviour
{
    private readonly int IsMoving = Animator.StringToHash("IsMoving");
    private readonly int IsRunning = Animator.StringToHash("IsRunning");
    private readonly int IsJumping = Animator.StringToHash("IsJumping");
    private readonly int IsFalling = Animator.StringToHash("IsFalling");
    private readonly int IsGrounded = Animator.StringToHash("IsGrounded");

    private Animator animator;

    private void Start()
    {
       animator = GetComponentInChildren<Animator>();
       if(animator == null)
        {
            Debug.LogWarning(this.name + ": Player Animator Not Found");
            this.enabled = false;
        }
    }
    public void Move()
    {
        animator.SetBool(IsMoving, true);
    }

    public void Stop() 
    {
        animator.SetBool(IsMoving, false);
    }

    public void Run()
    {
        animator.SetBool(IsRunning, true);
    }

    public void RunStop()
    {
        animator.SetBool(IsRunning, false);
    }

    public void Jump()
    {
        animator.SetTrigger(IsJumping);


        //if (animator.getcurrentanimatorstateinfo(0).isname("jump"))
        //{

        //}
        //animator.GetAnimatorTransitionInfo(0).IsName()
        //animator.IsInTrans


    }

    public void Fall()
    {
        animator.SetTrigger(IsFalling);
        animator.SetBool(IsGrounded, false);
    }

    public void Land()
    {
        animator.SetBool(IsGrounded, true);
    }
}

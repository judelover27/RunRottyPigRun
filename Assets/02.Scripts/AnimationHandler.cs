using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public Animator animator;
    protected int move = Animator.StringToHash("Move");
    protected int run = Animator.StringToHash("Run");
    protected int attack = Animator.StringToHash("Attack");
    protected int jump = Animator.StringToHash("Jump");
    protected int hit = Animator.StringToHash("Hit");
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Move() { }
    public virtual void Run() { }
    public virtual void Jump() { }
    public virtual void Attack() { }
    public virtual void Hit() { }

}

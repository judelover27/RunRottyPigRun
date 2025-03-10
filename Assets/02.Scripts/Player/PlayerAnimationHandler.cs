using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimationHandler : AnimationHandler
{
    protected int _float = Animator.StringToHash("Float");

    private bool isMoving;
    private bool isRunning;
    protected override void Start()
    {
        base.Start();

        if (CharacterManager.Instance?.Player?.condition != null)
        {
            Debug.Log("onTakeDamage 이벤트에 Hit() 등록");
            CharacterManager.Instance.Player.condition.onTakeDamage += Hit;
        }
        else
        {
            Debug.LogError("CharacterManager.Instance.Player.condition이 초기화되지 않음!");
        }
    }
    protected override void Update()
    {
        Move();
    }
    public override void Move() 
    {
        bool newIsRunning = CharacterManager.Instance.Player.controller.isRun;
        bool newIsMoving = CharacterManager.Instance.Player.controller.isMove;

        if (newIsMoving != isMoving)//애니메이션 상태가 변할때만 setbool
        {
            animator.SetBool(move, newIsMoving);
            isMoving = newIsMoving;
        }

        if (newIsRunning != isRunning)
        {
            animator.SetBool(run, newIsRunning);
            isRunning = newIsRunning;
        }
    }


    public override void Jump() 
    {
        animator.SetTrigger(jump);
    }
    public override void Attack() 
    {
        animator.SetTrigger(attack);
    }
    public override void Hit()
    {
        animator.SetTrigger(hit);
    }

    public void CheckFloat()
    {

    }
    public void Float(bool isFloat)
    {
        animator.SetBool(_float, isFloat);
    }
}

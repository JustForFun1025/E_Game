using UnityEngine;
using System.Collections;

public class PlayerIdleState : IState
{
    private Player Player = null;

    public PlayerIdleState(Player player)
    {
        Player = player;
    }

    public int GetStateID()
    {
        return (int)Player.PlayerState.Idle;
    }

    public void OnEnter(StateMachine stateMachine, IState prevState, object param1, object param2)
    {
        Debug.Log("进入Idle状态 上次的状态为 ：" + prevState);
        Player.PlayerAnimator.SetTrigger("idle"); 
    }

    public void OnLeave(IState nextState, object param1, object param2)
    {
        Debug.Log("退出Idle状态 下次的状态为 ：" + nextState);
    }

    public void OnUpdate()
    {
    }

    public void OnFixedUpdate()
    {
    }

    public void OnLateUpdate()
    {
    }

}
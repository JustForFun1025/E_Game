using UnityEngine;
using System.Collections;

public class PlayerWalkState : IState
{
    private Player Player = null;

    public PlayerWalkState(Player player)
    {
        Player = player;
    }

    public int GetStateID()
    {
        return (int)Player.PlayerState.Walk;
    }

    public void OnEnter(StateMachine stateMachine, IState prevState, object param1, object param2)
    {
        Debug.Log("进入待机状态 上次的状态为 ：" + prevState);
        Player.PlayerAnimator.SetTrigger("walk");
    }

    public void OnLeave(IState nextState, object param1, object param2)
    {
        Debug.Log("退出待机状态 下次的状态为 ：" + nextState);
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
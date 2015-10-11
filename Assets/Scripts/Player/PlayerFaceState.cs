using UnityEngine;
using System.Collections;

public class PlayerFaceState : IState
{
    private Player Player = null;

    public PlayerFaceState(Player player)
    {
        Player = player;
    }

    public int GetStateID()
    {
        return (int)Player.PlayerState.Idle;
    }

    public void OnEnter(StateMachine stateMachine, IState prevState, object param1, object param2)
    {
        Player.gameObject.GetComponent<Animation>().Play("zombie_walk");
        Debug.Log("进入待机状态 上次的状态为 ：" + prevState);
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
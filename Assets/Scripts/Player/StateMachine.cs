using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 状态机管理脚本
/// </summary>
public class StateMachine
{
    /// <summary>
    /// 存储所有注册进来的状态。key是状态ID，value是状态对象
    /// </summary>
    private Dictionary<int, IState> StateDictionary;

    /// <summary>
    /// 当前运行的状态
    /// </summary>
    private IState CurrentStatus;

    public StateMachine()
    {
        CurrentStatus = null;
        StateDictionary = new Dictionary<int, IState>();
    }

    /// <summary>
    /// 注册一个状态
    /// </summary>
    /// <param name="state">要注册的状态</param>
    /// <returns>成功返回true，如果此状态ID已存在或状态为NULL，则返回false</returns>
    public bool RegistState(IState state)
    {
        if (null == state)
        {
            Debug.LogWarning("StateMachine::RegistState->state is null");
            return false;
        }

        if (StateDictionary.ContainsKey(state.GetStateID()))
        {
            Debug.LogWarning("StateMachine::RegistState->state had exist! state id=" + state.GetStateID());
            return false;
        }

        StateDictionary[state.GetStateID()] = state;

        return true;
    }

    /// <summary>
    /// 尝试获取一个状态
    /// </summary>
    /// <param name="stateId"></param>
    /// <returns></returns>
    public IState GetState(int stateId)
    {
        IState ret = null;
        StateDictionary.TryGetValue(stateId, out ret);
        return ret;
    }

    /// <summary>
    /// 停止当前正在运行的状态, 切换到空状态
    /// </summary>
    public void StopState(object param1, object param2)
    {
        if (null == CurrentStatus)
        {
            return;
        }

        CurrentStatus.OnLeave(null, param1, param2);

        CurrentStatus = null;
    }

    /// <summary>
    /// 取消状态的注册
    /// </summary>
    /// <param name="stateID">要取消的状态ID</param>
    /// <returns>如果找不到状态或状态正在运行，则会返回false</returns>
    public bool CancelState(int stateID)
    {
        if (!StateDictionary.ContainsKey(stateID))
        {
            return false;
        }

        if (null != CurrentStatus && CurrentStatus.GetStateID() == stateID)
        {
            return false;
        }

        return StateDictionary.Remove(stateID);
    }

    public delegate void BetweenSwitchState(IState from, IState to, object param1, object param2);

    /// <summary>
    /// 在切换状态之间回调
    /// </summary>
    public BetweenSwitchState BetweenSwitchStateCallBack { get; set; }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="newStateID">要切换的新状态</param>
    /// <returns>如果找不到新的状态，或者新旧状态一样，返回false</returns>
    public bool SwitchState(int newStateID, object param1, object param2)
    {
        //状态一样，不做转换//
        if (null != CurrentStatus && CurrentStatus.GetStateID() == newStateID)
        {
            return false;
        }

        IState newState = null;
        StateDictionary.TryGetValue(newStateID, out newState);
        if (null == newState)
        {
            return false;
        }

        IState oldState = CurrentStatus;

        if (null != oldState)
        {
            oldState.OnLeave(newState, param1, param2);
        }

        if (BetweenSwitchStateCallBack != null) BetweenSwitchStateCallBack(oldState, newState, param1, param2);

        CurrentStatus = newState;

        if (null != newState)
        {
            newState.OnEnter(this, oldState, param1, param2);
        }

        return true;
    }

    /// <summary>
    /// 获取当前状态
    /// </summary>
    /// <returns></returns>
    public IState GetCurState()
    {
        return CurrentStatus;
    }

    /// <summary>
    /// 获取当前状态ID
    /// </summary>
    /// <returns></returns>
    public int GetCurStateID()
    {
        IState state = GetCurState();
        return (null == state) ? 0 : state.GetStateID();
    }

    /// <summary>
    /// 判断当前是否在某个状态下
    /// </summary>
    /// <param name="stateID"></param>
    /// <returns></returns>
    public bool IsInState(int stateID)
    {
        if (null == CurrentStatus)
        {
            return false;
        }

        return CurrentStatus.GetStateID() == stateID;
    }

    /// <summary>
    /// 每帧的更新回调
    /// </summary>
    public void OnUpdate()
    {
        if (null != CurrentStatus)
        {
            CurrentStatus.OnUpdate();
        }
    }

    /// <summary>
    /// 每帧的更新回调
    /// </summary>
    public void OnFixedUpdate()
    {
        if (null != CurrentStatus)
        {
            CurrentStatus.OnFixedUpdate();
        }
    }

    /// <summary>
    /// 每帧的更新回调
    /// </summary>
    public void OnLateUpdate()
    {
        if (null != CurrentStatus)
        {
            CurrentStatus.OnLateUpdate();
        }
    }
}
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Idle = 0,

        /// <summary>
        /// 转变面向
        /// </summary>
        Face,

        /// <summary>
        /// 步行
        /// </summary>
        Walk,

        /// <summary>
        /// 爬
        /// </summary>
        Climb,
    }

    public Animator PlayerAnimator { get; private set; }
    private StateMachine PlayerStateMachine;
    private PlayerState CurrentState;
    

    private void Awake()
    {
        //注册所有状态//
        PlayerStateMachine.RegistState(new PlayerIdleState(this));
        PlayerStateMachine.RegistState(new PlayerFaceState(this));
        PlayerStateMachine.RegistState(new PlayerWalkState(this));
        PlayerStateMachine.RegistState(new PlayerClimbState(this));
        PlayerStateMachine.SwitchState((int)PlayerState.Idle, null, null);
    }

    private void Start()
    {
        PlayerStateMachine = new StateMachine();
        PlayerAnimator = GetComponent<Animator>();
        CurrentState = PlayerState.Idle;
    }

    private void Update()
    {
        if (PlayerStateMachine.GetCurState() != null)
        {
            CurrentState = (PlayerState)PlayerStateMachine.GetCurStateID();
        }
        PlayerStateMachine.OnUpdate();
    }

    private void FixedUpdate()
    {
        PlayerStateMachine.OnFixedUpdate();
    }

    private void LateUpdate()
    {
        PlayerStateMachine.OnLateUpdate();
    }
}
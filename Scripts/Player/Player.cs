using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private struct Consts
    {
        public const float FacingTime = 0.1f;
        public const float WalkTime = 1.0f;
        public const float MoveUnit = 0.125f;
    }

    private enum Status
    {
        Lock,
        Idle,
        Walk,
        Climb,
    }

    private enum Facing
    {
        Up,
        Down,
        Left,
        Right,
    }

    private struct TimeSwitch
    {
        public bool Facing;
        public bool Walk;
    }

    private Rigidbody2D PlayerRigibody;
    private Facing PlayerFacing; 
    private Animator PlayerAnimator;
    private Status PlayerStatus;
    private TimeSwitch TimeSwitchs;
    private float TimeInterval;

    private void Start()
    {
        TimeSwitchs.Facing = false;
        TimeSwitchs.Walk = false;
        PlayerRigibody = GetComponent<Rigidbody2D>();
        PlayerFacing = Facing.Right;
        PlayerAnimator = GetComponent<Animator>();
        PlayerStatus = Status.Idle;
        TimeInterval = 0f;

        Controller.AddInputMonitor("Up", EventController);
        Controller.AddInputMonitor("Down", EventController);
        Controller.AddInputMonitor("Left", EventController);
        Controller.AddInputMonitor("Right", EventController);
        Controller.AddInputMonitor("Default", EventController);
    }

    private void EventController(string tag)
    {
        if (PlayerStatus == Status.Idle)
        {
            switch (tag)
            {
                case "Up":
                case "Down":
                case "Left":
                case "Right":
                    DoWalkOrClimb(tag);
                    break;
            }
        }
    }

    private void DoIdle()
    {
        PlayerAnimator.SetTrigger("Idle");
        if (PlayerFacing != Facing.Left) PlayerFacing = Facing.Right;       //TODO BATTER
        PlayerRigibody.velocity = Vector3.zero;
        Vector3 position = transform.position;
        transform.position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), 0);
    }
    private void DoWalkOrClimb(string tag)
    {
        switch (tag)
        {
            case "Up":
                if (PlayerFacing == Facing.Up)
                {
                    TimeSwitchs.Walk = true;
                    PlayerStatus = Status.Climb;
                    PlayerAnimator.SetTrigger("Climb");
                    PlayerRigibody.velocity = Vector3.up;
                }
                else
                {
                    TimeSwitchs.Facing = true;
                    PlayerStatus = Status.Lock;
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    PlayerFacing = Facing.Up;
                }
                
                break;
            case "Down":
                if (PlayerFacing == Facing.Down)
                {
                    TimeSwitchs.Walk = true;
                    PlayerStatus = Status.Climb;
                    PlayerAnimator.SetTrigger("Climb");
                    PlayerRigibody.velocity = Vector3.down;
                }
                else
                {
                    TimeSwitchs.Facing = true;
                    PlayerStatus = Status.Lock;
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    PlayerFacing = Facing.Down;
                }
                break;
            case "Left":
                if (PlayerFacing == Facing.Left)
                {
                    TimeSwitchs.Walk = true;
                    PlayerStatus = Status.Walk;
                    PlayerAnimator.SetTrigger("Walk");
                    PlayerRigibody.velocity = Vector3.left;
                }
                else
                {
                    TimeSwitchs.Facing = true;
                    PlayerStatus = Status.Lock;
                    transform.rotation = new Quaternion(0, 180,0,0);
                    PlayerFacing = Facing.Left;
                }
                break;
            case "Right":
                if (PlayerFacing == Facing.Right)
                {
                    TimeSwitchs.Walk = true;
                    PlayerStatus = Status.Walk;
                    PlayerAnimator.SetTrigger("Walk");
                    GetComponent<Rigidbody2D>().velocity = Vector3.right;
                }
                else
                {
                    TimeSwitchs.Facing = true;
                    PlayerStatus = Status.Lock;
                    if (PlayerFacing == Facing.Left) transform.rotation = new Quaternion(0, 0, 0, 0);
                    PlayerFacing = Facing.Right;
                }
                break;
        }
    }
    private bool Timing(float TimeToJudge)      //共用TimeInterval，所以请保证调用Timing的方法唯一。
    {
        if (TimeInterval > TimeToJudge)     
        {
            TimeInterval = 0f;
            return true;
        }
        TimeInterval += Time.deltaTime;
        return false;
    }

    private void FixedUpdate()
    {
        if (TimeSwitchs.Walk && Timing(Consts.WalkTime))
        {
            TimeSwitchs.Walk = false;
            PlayerStatus = Status.Idle;
            DoIdle();
        }
        else if (TimeSwitchs.Facing && Timing(Consts.FacingTime))
        {
            TimeSwitchs.Facing = false;
            PlayerStatus = Status.Idle;
        }
    }
}
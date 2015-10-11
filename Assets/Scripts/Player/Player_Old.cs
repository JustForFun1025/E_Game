using UnityEngine;
using System.Collections;

public class PlayerOld : MonoBehaviour
{
    private struct Consts
    {
        public const float FacingTime = 0.2f;
        public const float WalkTime = 0.50f;
        public const float MoveSpeed = 2f;
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
        public bool GetInput;
        //public bool Facing;
        //public bool Walk;
    }

    private Sprite IdleSprite;
    private Sprite ClimbSprite;
    private Sprite PlayerSprite;
    private Rigidbody2D PlayerRigibody;
    private Facing PlayerFacing; 
    private Animator PlayerAnimator;
    private Status PlayerStatus;
    private TimeSwitch TimeSwitchs;
    private float TimeInterval;
    private float TimeToWait;
    private bool CanHandleInput;
    private Collider2D ObjectInFront;

    private void Start()
    {
        //TimeSwitchs.Facing = false;
        //TimeSwitchs.Walk = false;
        PlayerSprite = GetComponent<SpriteRenderer>().sprite;
        IdleSprite = Sprite.Create((Texture2D)Resources.Load(@"Player/idle"), PlayerSprite.textureRect, new Vector2(0.5f, 0.5f));//注意居中显示采用0.5f值
        ClimbSprite = Sprite.Create((Texture2D)Resources.Load(@"Player/climb"), PlayerSprite.textureRect, new Vector2(0.5f, 0.5f));
        PlayerRigibody = GetComponent<Rigidbody2D>();
        PlayerFacing = Facing.Right;
        PlayerAnimator = GetComponent<Animator>();
        PlayerStatus = Status.Idle;
        TimeInterval = 0f;
        TimeToWait = 0f;
        CanHandleInput = true;
        ObjectInFront = null;

        Controller.AddInputMonitor("Up", EventController);
        Controller.AddInputMonitor("Down", EventController);
        Controller.AddInputMonitor("Left", EventController);
        Controller.AddInputMonitor("Right", EventController);
        Controller.AddInputMonitor("Default", EventController);
    }

    private void EventController(string tag)
    {
        if (CanHandleInput)
        {
            CanHandleInput = false;
            switch (tag)
            {
                case "Up":
                case "Down":
                case "Left":
                case "Right":
                    JudgeWalkOrClimbOrPush(tag);
                    break;
            }  
        }
    }

    //Status Judgement
    private void JudgeWalkOrClimbOrPush(string tag)
    {
        if (tag == PlayerFacing.ToString())
        {
            if (ObjectInFront && ObjectInFront.tag == "Wall") DoIdle();
            else DoWalkOrClimb(tag);
        }
        else
        {
            FaceTo(tag);
        }
    }

    //Status Implement
    private void FaceTo(string tag)
    {
        switch (tag)
        {
            case "Up":
                    TimeToWait = Consts.FacingTime;
                    TimeSwitchs.GetInput = true;
                    PlayerStatus = Status.Lock;
                    PlayerFacing = Facing.Up;
                    Debug.Log(ClimbSprite);
                    PlayerSprite = ClimbSprite;
                break;
            case "Down":
                    TimeToWait = Consts.FacingTime;
                    TimeSwitchs.GetInput = true;
                    PlayerStatus = Status.Lock;
                    //ClimbSprite.textureRect.Set(PlayerSprite.textureRect.x,PlayerSprite.textureRect.y,PlayerSprite.textureRect.width,PlayerSprite.textureRect.height);
                    PlayerSprite = ClimbSprite;
                    PlayerFacing = Facing.Down;
                break;
            case "Left":
                    TimeToWait = Consts.FacingTime;
                    TimeSwitchs.GetInput = true;
                    PlayerStatus = Status.Lock;
                    //IdleSprite.textureRect.Set(PlayerSprite.textureRect.x,PlayerSprite.textureRect.y,PlayerSprite.textureRect.width,PlayerSprite.textureRect.height);
                    PlayerSprite = IdleSprite;
                    transform.rotation = new Quaternion(0, 180, 0, 0);
                    PlayerFacing = Facing.Left;
                break;
            case "Right":
                    TimeToWait = Consts.FacingTime;
                    TimeSwitchs.GetInput = true;
                    PlayerStatus = Status.Lock;
                    //IdleSprite.textureRect.Set(PlayerSprite.textureRect.x,PlayerSprite.textureRect.y,PlayerSprite.textureRect.width,PlayerSprite.textureRect.height);
                    PlayerSprite = IdleSprite;
                    if (PlayerFacing == Facing.Left) transform.rotation = new Quaternion(0, 0, 0, 0);
                    PlayerFacing = Facing.Right;
                break;
        }
    }

    private void DoIdle()
    {
        PlayerAnimator.SetTrigger("Idle");
        PlayerRigibody.velocity = Vector3.zero;
        Vector3 position = transform.position;
        transform.position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), 0);
    }
    private void DoWalkOrClimb(string tag)
    {
        switch (tag)
        {
            case "Up":
                TimeToWait = Consts.WalkTime;
                TimeSwitchs.GetInput = true;
                PlayerStatus = Status.Climb;
                PlayerAnimator.SetTrigger("Climb");
                PlayerRigibody.velocity = new Vector2(0, Consts.MoveSpeed);
                break;
            case "Down":
                TimeToWait = Consts.WalkTime;
                TimeSwitchs.GetInput = true;
                PlayerStatus = Status.Climb;
                PlayerAnimator.SetTrigger("Climb");
                PlayerRigibody.velocity = new Vector2(0, -Consts.MoveSpeed);
                break;
            case "Left":
                TimeToWait = Consts.WalkTime;
                TimeSwitchs.GetInput = true;
                PlayerStatus = Status.Walk;
                PlayerAnimator.SetTrigger("Walk");
                PlayerRigibody.velocity = new Vector2(-Consts.MoveSpeed, 0f);
                break;
            case "Right":
                TimeToWait = Consts.WalkTime;
                TimeSwitchs.GetInput = true;
                PlayerStatus = Status.Walk;
                PlayerAnimator.SetTrigger("Walk");
                GetComponent<Rigidbody2D>().velocity = new Vector2(Consts.MoveSpeed, 0f);
                break;
        }
    }

    private void DoPushStone()
    {

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        ObjectInFront = other;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (ObjectInFront == other)     ObjectInFront = null;
    }

    private void FixedUpdate()
    {
        if (TimeSwitchs.GetInput && Timing(TimeToWait))
        {
            //DoIdle();
            TimeSwitchs.GetInput = false;
            CanHandleInput = true;
        }
    }
}
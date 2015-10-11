using UnityEngine;
using System.Collections;

public class Stone : MonoBehaviour
{

    private struct Consts
    {
        public const float FixedDistance = 0.2f;
        public const float TimeToJudgeScroll = 0.2f;
        public const float TimeToReadyScroll = 0.1f;
        public const float TimeToScroll = 1.0f;
    }

    private struct SupportedObject
    {
        public Collider2D Down;
        public Collider2D Left;
        public Collider2D Right;
        public Collider2D LeftDown;
        public Collider2D RightDown;
    }

    private string[] UnstableTagList;
    private Transform StoneImage;
    private Rigidbody2D StoneRigibody;
    private SupportedObject Supported;
    private bool Unstable;          //不平稳状态时，石头会向左右空位滚动。
    private bool DropDown;
    private bool ScrolLeft;       
    private bool ScrolRight;
    private bool ReadyScrol;
    private float TimeInterval;

    private void Start()
    {
        UnstableTagList = new string[] { "Stone" };        // ### 不平稳承载物的标签请在这里注明。
        StoneImage = transform.FindChild("image");
        StoneRigibody = GetComponent<Rigidbody2D>();
        Supported.Down = null;
        Supported.Left = null;
        Supported.Right = null;
        Supported.LeftDown = null;
        Supported.RightDown = null;
        Unstable = false;
        DropDown = false;
        ScrolLeft = false;
        ScrolRight = false;
        ReadyScrol = false;
        TimeInterval = 0f;
    }

    private void FixedPosition()
    {
        Vector3 position = transform.position;
        transform.position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), 0);
    }

    private bool IsUnstable(Collider2D obj)
    {
        foreach (string tag in UnstableTagList)
            if (obj.tag == tag) return true;
        return false;
    }

    private bool JudgeSameX(Collider2D supported)
    {
        if (Mathf.Abs(supported.transform.position.x - transform.position.x) < Consts.FixedDistance)
            return true;
        else
            return false;
    }
    private bool JudgeSameY(Collider2D supported)
    {
        if (Mathf.Abs(supported.transform.position.y - transform.position.y) < Consts.FixedDistance)
            return true;
        else
            return false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log("SomeThing Enter：");
        //Debug.Log(other.name);
        
        if ( !Supported.Down )
        {
            if ( other.transform.position.y < transform.position.y && JudgeSameX(other))
            {
                Debug.Log("#Support Down");
                DropDown = false;
                Supported.Down = other;
                if (IsUnstable(other)) Unstable = true;
                else Unstable = false;
                StoneRigibody.velocity = Vector3.zero;
                FixedPosition();
            }
        }
        else if (Unstable)
        {
            if (!Supported.Left && other.transform.position.x < transform.position.x && JudgeSameY(other))
            {
                Debug.Log("#Support Left");
                Supported.Left = other;
                //FixedPosition();
            }
            if (!Supported.LeftDown  && other.transform.position.x < transform.position.x && !JudgeSameX(other) )
            {
                Debug.Log("#Support LeftDown");
                Supported.LeftDown = other;
                //FixedPosition();
            }
            if (!Supported.Right && other.transform.position.x > transform.position.x && JudgeSameY(other))
            {
                Debug.Log("#Support Right");
                Supported.Right = other;
                //FixedPosition();
            }
            if (!Supported.RightDown  && other.transform.position.x > transform.position.x && !JudgeSameX(other))
            {
                Debug.Log("#Support RightDown");
                Supported.RightDown = other;
                //FixedPosition();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Supported Exit");
        Debug.Log(other.name);
        if (other == Supported.Down) Supported.Down = null;
        else if (other == Supported.Left) Supported.Left = null;
        else if (other == Supported.Right) Supported.Right = null;
        else if (other == Supported.LeftDown) Supported.LeftDown = null;
        else if (other == Supported.RightDown) Supported.RightDown = null;
    }

    private void CheckSupported()           //检查支撑物状态
    {
        // 1判断下是否存在
        if (Supported.Down)
        {
            if (!JudgeSameX(Supported.Down))
            {
                Debug.Log("【Down Invalid】");
                Supported.Down = null;
            }
            // 2判断下是否不平整
            else if (Unstable)
            {
                // 3判断是否会滚动
                if (Supported.Left)
                {
                    if (!JudgeSameY(Supported.Left))
                    {
                        Debug.Log("【Left Invalid】");
                        Supported.Left = null;
                    }
                }
                else if (Supported.LeftDown)
                {
                    if (JudgeSameX(Supported.LeftDown) || JudgeSameY(Supported.LeftDown))
                    {
                        Debug.Log("【LeftDown Invalid】");
                        Supported.LeftDown = null;
                    }
                }
                else
                {
                    Debug.Log("【Scrolling Left】");
                    ScrolLeft = true;
                    return;
                }
                if (Supported.Right)
                {
                    if (!JudgeSameY(Supported.Right))
                    {
                        Debug.Log("【Right Invalid】");
                        Supported.Right = null;
                    }
                }
                else if (Supported.RightDown)
                {
                    if (JudgeSameX(Supported.RightDown) || JudgeSameY(Supported.RightDown))
                    {
                        Debug.Log("【RightDown Invalid】");
                        Supported.RightDown = null;
                    }
                }
                else
                {
                    Debug.Log("【Scrolling Right】");
                    ScrolRight = true;
                }

            }
        }
        else
        {
            Debug.Log("【DropDown】");
            DropDown = true;
        }
    }

    private void Scrolling()
    {
        if (ScrolLeft)
            StoneRigibody.velocity = Vector3.left;
        else if (ScrolRight)
            StoneRigibody.velocity = Vector3.right;
    }

    private bool Timing(float TimeToJudge)
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
        if (!DropDown && !ScrolLeft && !ScrolRight)     //非运动状态时检测支撑物状态
        {
            if (Timing(Consts.TimeToJudgeScroll)) CheckSupported();
        }
        else
        {
            if (DropDown) StoneRigibody.velocity = Vector3.down;
            else if (!ReadyScrol)
            {
                if (Timing(Consts.TimeToReadyScroll)) ReadyScrol = true;
            }
            else
            {
                Scrolling();
                if (Timing(Consts.TimeToScroll))
                {
                    ScrolLeft = false;
                    ScrolRight = false;
                    StoneRigibody.velocity = Vector3.zero;
                }
            }
        }
    }
}

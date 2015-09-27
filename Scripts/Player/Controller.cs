using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Controller : MonoBehaviour
{
    private struct Consts
    {
    }
    private enum OperateType
    {
        Default,
        Up,
        Down,
        Left,
        Right,
    }

    private OperateType InputCache;
    private static InputEvent InputEvents = new InputEvent();

    private Controller() { }

    private void Start()
    {
        InputCache = OperateType.Default;
    }

    private bool CheckInputCacheChange()
    {
        if (Input.GetKey("w")) InputCache = OperateType.Up;
        else if (Input.GetKey("s")) InputCache = OperateType.Down;
        else if (Input.GetKey("a")) InputCache = OperateType.Left;
        else if (Input.GetKey("d")) InputCache = OperateType.Right;
        else if (Input.GetKeyUp("w") || Input.GetKeyUp("s") || Input.GetKeyUp("a") || Input.GetKeyUp("d")) InputCache = OperateType.Default;
        else return false;
        return true;
    }

    private void DoEvents(string tag)
    {
        if (InputEvents.Contains(tag) )
        {
            var dEvent = InputEvents[tag];
            while ( dEvent != null )
            {
                dEvent.Value(tag);
                dEvent = dEvent.Next;
            }
        }
    }

    private void Update()
    {
        if (CheckInputCacheChange())    DoEvents(InputCache.ToString());
    }

    // Public Function

    /// <summary>
    /// 监听用户输入，以带输入类型标签回调所提供的函数。
    /// </summary>
    /// <param name="tag">输入类型标签如："Up","Down","Left","Right"...</param>
    /// <param name="dEvent">相应的触发事件如:doWalk</param>
    public static void AddInputMonitor(string tag,InputEvent.DEvent dEvent)
    {
        InputEvents.Add(tag, dEvent);
    }
}

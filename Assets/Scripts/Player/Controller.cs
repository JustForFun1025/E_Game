﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Controller : MonoBehaviour
{
    private struct Consts
    {
        public const float TimeIntervalToJudgeInput = 1.0f;
    }
    private enum OperateType
    {
        Default,
        Up,
        Down,
        Left,
        Right,
    }
    private struct InputCache
    {
        public static OperateType CurrentInput;
        public static OperateType NextInput;
        public static bool IsContinuous;
    }

    private float TimeInterval;
    private static InputEvent InputEvents = new InputEvent();

    private Controller() { }

    private void Start()
    {
        TimeInterval = 0;
        InputCache.CurrentInput = OperateType.Default;
        InputCache.NextInput = OperateType.Default;
        InputCache.IsContinuous = false;
    }

    private void SaveInputToCache()
    {
        if (Input.GetKey("w")) InputCache.NextInput = OperateType.Up;
        else if (Input.GetKey("s")) InputCache.NextInput = OperateType.Down;
        else if (Input.GetKey("a")) InputCache.NextInput = OperateType.Left;
        else if (Input.GetKey("d")) InputCache.NextInput = OperateType.Right;
        else if (Input.GetKeyUp("w") || Input.GetKeyUp("s") || Input.GetKeyUp("a") || Input.GetKeyUp("d")) InputCache.IsContinuous = false;
    }

    private bool TimeToHandleInput()
    {
        if (TimeInterval > Consts.TimeIntervalToJudgeInput)
        {
            TimeInterval -= Consts.TimeIntervalToJudgeInput;
            return true;
        }
        TimeInterval += Time.deltaTime;
        return false;
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
        SaveInputToCache();
        
    }
    private void FixedUpdate()
    {
        if (TimeToHandleInput())
        {
            DoEvents(InputCache.NextInput.ToString());
            if (!InputCache.IsContinuous) InputCache.NextInput = OperateType.Default;
        }
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

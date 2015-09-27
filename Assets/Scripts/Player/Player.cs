using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private struct Consts
    {
        
    }
    public enum StatusType
    {
        Idle,
        Walk,
    }

    private static StatusType Status;

    private Player() { }

    private void Start()
    {
        Status = StatusType.Idle; 
    }

    private void Update()
    {
        //if (Status == Consts.StatusType.Walk)
        //{
        //    GetComponent<Animator>().SetBool("Idle", false);
        //    GetComponent<Animator>().SetBool("Walk", true);
        //}
        //else if (Status == Consts.StatusType.Idle)
        //{
        //    GetComponent<Animator>().SetBool("Walk", false);
        //    GetComponent<Animator>().SetBool("Idle", true);
        //}
    }

    public static void SetStatusToIdle()
    {
        Status = StatusType.Idle;
    }
    public static void SetStatusToWalk()
    {
        Status = StatusType.Walk;
    }
}

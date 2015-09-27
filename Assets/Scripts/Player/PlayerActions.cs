using UnityEngine;
using System.Collections;

public class PlayerActions : MonoBehaviour {
    private struct Consts
    {
        public const float MoveUnit = 0.125f;
    }

    private enum Actions
    {
        Idle,
        Walk,
        Climb,
    }

	public void DoIdle()
    {

    }
    private void DoWalk(string tag)
    {
        GetComponent<Animator>().SetBool("Walk", true);
        Vector3 position = GetComponent<Rigidbody2D>().position;
        switch (tag)
        {
            case "Up":
                GetComponent<Rigidbody2D>().position = new Vector3(position.x, position.y + Consts.MoveUnit, position.z);
                break;
            case "Down":
                GetComponent<Rigidbody2D>().position = new Vector3(position.x, position.y - Consts.MoveUnit, position.z);
                break;
            case "Left":
                GetComponent<Rigidbody2D>().velocity = Vector3.left;
                break;
            case "Right":
                GetComponent<Rigidbody2D>().velocity = Vector3.right;
                break;
            default:
                //
                break;
        }
    }
    
    private void DoIdle(string tag)
    {
        GetComponent<Animator>().SetBool("Walk", false);
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    void Start()
    {
        Controller.AddInputMonitor("Up", DoWalk);
        Controller.AddInputMonitor("Down", DoWalk);
        Controller.AddInputMonitor("Left", DoWalk);
        Controller.AddInputMonitor("Right", DoWalk);
        Controller.AddInputMonitor("Default", DoIdle);
    }
}

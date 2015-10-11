using UnityEngine;
using System.Collections;

public class Grass : MonoBehaviour {

    private const float TimeOfDestory = 0.5f;

    private float TimeInterval;
    private bool Destoryed;
    private Animator GrassAnimator;
    
    private void Start()
    {
        TimeInterval = 0f;
        Destoryed = false;
        GrassAnimator = GetComponent<Animator>();
        GrassAnimator.StopRecording();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("!!!!!!!!");
        if (other.collider.tag == "Player")
        {
            GrassAnimator.SetTrigger("Destory");
            Destoryed = true;
        }
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

    private void Update()
    {
        if (Destoryed && Timing(TimeOfDestory)) Destroy(transform.gameObject);
    }
    
}

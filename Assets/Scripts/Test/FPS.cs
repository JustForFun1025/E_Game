using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour
{
    public float UpdateInterval = 0.5F;
    private float LastInterval;
    private int Frames = 0;
    private float Fps;
    void Start()
    {
        LastInterval = Time.realtimeSinceStartup;
        Frames = 0;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 200, 200), "FPS:" + Fps.ToString("f2"));
    }

    void Update()
    {
        ++Frames;

        if (Time.realtimeSinceStartup > LastInterval + UpdateInterval)
        {
            Fps = Frames / (Time.realtimeSinceStartup - LastInterval);
            Frames = 0;
            LastInterval = Time.realtimeSinceStartup;
        }
    }
}
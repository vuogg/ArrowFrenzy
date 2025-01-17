using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;

    //private void Update()
    //{
    //    Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
    //    Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    //}

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}

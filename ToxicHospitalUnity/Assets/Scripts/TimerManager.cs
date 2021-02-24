using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private bool isPaused = true;
    public bool IsPaused { get { return isPaused; } private set { isPaused = value; } }

    private float timerDuration = 0.0f;
    private float timeRemaining = 0.0f;

    private bool isFinished = false;
    private bool loopTimer = false;

    /// <summary>
    /// Creates a timer. If using TimerManager, use the CreateNewTimer function.
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="startRunning"></param>
    public Timer(float duration, bool startRunning = false)
    {
        timerDuration = duration;
        timeRemaining = duration;
        isPaused = !startRunning;
    }

    /// <summary>
    /// Updates the timer's progress. Do not call this if using the TimerManager.
    /// </summary>
    /// <param name="dt"></param>
    public void UpdateTime(float dt)
    {
        if (isFinished && loopTimer)
        {
            isFinished = false;
        }
        if (!IsPaused && !isFinished)
        {
            timeRemaining -= dt;
            if (timeRemaining <= 0.0f)
            {
                if (loopTimer)
                {
                    timeRemaining += timerDuration;
                }
                else
                {
                    Pause();
                }
                isFinished = true;
            }
        }
    }

    public void Pause()
    {
        IsPaused = true;
    }

    public void Unpause()
    {
        IsPaused = false;
    }

    public void Restart()
    {
        Unpause();
        timeRemaining = timerDuration;
        isFinished = false;
    }

    public bool IsFinished { get { return isFinished; } }

    /// <summary>
    /// Returns percentage of completion as a float from 0 to 1
    /// </summary>
    /// <returns></returns>
    public float Progress
    {
        get { return Mathf.Min(1.0f, 1.0f - (timeRemaining / timerDuration)); }
    }
}
public class TimerManager : MonoBehaviour
{
    private static TimerManager instance;
    public static TimerManager Instance { get { return instance; } }

    private List<Timer> timers = new List<Timer>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public Timer CreateNewTimer(float duration, bool startRunning = false)
    {
        timers.Add(new Timer(duration, startRunning));
        return timers[timers.Count - 1];
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Timer t in timers)
        {
            t.UpdateTime(Time.deltaTime);
        }
    }
}

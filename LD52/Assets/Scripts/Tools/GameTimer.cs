using System.Diagnostics;
using UnityEngine;
public class GameTimer
{

    Stopwatch stopwatch;
    public GameTimer()
    {
        stopwatch = Stopwatch.StartNew();
    }
    public void Pause()
    {
        if (stopwatch.IsRunning)
        {
            stopwatch.Stop();
        }
    }

    public void Unpause()
    {
        stopwatch.Start();
    }

    public string GetString()
    {
        if (stopwatch.Elapsed.Hours > 0)
        {
            return stopwatch.Elapsed.ToString(@"hh\:mm\:ss\.ff");
        }
        else
        {
            return stopwatch.Elapsed.ToString(@"mm\:ss\.ff");
        }
    }

    public double GetTime()
    {
        return stopwatch.Elapsed.TotalMilliseconds;
    }

}
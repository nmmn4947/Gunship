using System;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class FramePerformance : MonoBehaviour
{
    private Stopwatch stopwatch = new Stopwatch();
    private bool once = false;
    private double ANCHORTIME = 0;
    private int ANCHORFRAMEPERSECONDS = 0;
    
    [HideInInspector] public float realTimeMEAN;
    [HideInInspector] public float realTimeMEDIAN;
    [HideInInspector] public float realTimeWorst = float.MaxValue;
    
    public int[] frameCountEachSecond = new int[20];
    private int eachSecondFrameCount = 0;
    private int meanSum = 0;
    private int tenFrameIndex = 0;
    
    private void Start()
    {
        Debug.Log("IsHighResolution = " + Stopwatch.IsHighResolution);
        ANCHORTIME = 0;
        ANCHORFRAMEPERSECONDS = Time.frameCount;
    }

    private void Update()
    {
        if (!once)
        {
            if (stopwatch.IsRunning)
            {
                stopwatch.Stop();
            }
            stopwatch.Start();
            once = true;
        }
        OneSecondTrigger();
    }

    private void OneSecondTrigger()
    {
        if (stopwatch.Elapsed.Seconds - ANCHORTIME >= 1)
        {
            eachSecondFrameCount = Time.frameCount - ANCHORFRAMEPERSECONDS;
            
            frameCountEachSecond[tenFrameIndex%20] = eachSecondFrameCount;
            
            //mean
            if (tenFrameIndex < 10)
            {
                meanSum += frameCountEachSecond[tenFrameIndex % 20];
                realTimeMEAN = meanSum/(tenFrameIndex + 1);
            }
            else
            {
                meanSum += frameCountEachSecond[tenFrameIndex % 20];
                meanSum -= frameCountEachSecond[(tenFrameIndex - 10) % 20];
                realTimeMEAN = meanSum/10;
            }
            
            //median
            if (tenFrameIndex < 10)
            {
                realTimeMEDIAN = frameCountEachSecond[(tenFrameIndex/2)%20];
            }
            else
            {
                realTimeMEDIAN = frameCountEachSecond[(tenFrameIndex - 5)%20];
            }
            
            //worst
            realTimeWorst = float.MaxValue;
            if (tenFrameIndex < 10)
            {
                for (int i = 0; i <= tenFrameIndex; i++) //bad performance??
                {
                    if (realTimeWorst > frameCountEachSecond[(i) % 20])
                    {
                        realTimeWorst = frameCountEachSecond[(i) % 20];
                    }
                }
            }
            else
            {
                for (int i = 0; i < 10; i++) //bad performance??
                {
                    if (realTimeWorst > frameCountEachSecond[(tenFrameIndex - i) % 20])
                    {
                        realTimeWorst = frameCountEachSecond[(tenFrameIndex - i) % 20];
                    }
                }
            }

            
            tenFrameIndex++;
            ANCHORTIME = stopwatch.Elapsed.Seconds;
            ANCHORFRAMEPERSECONDS = Time.frameCount;
        }
    }
}

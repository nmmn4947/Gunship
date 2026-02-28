using System;
using UnityEngine;

public class FramePerformanceOLD : MonoBehaviour
{
    private System.DateTime STARTTIME;// stopwatch
    private System.TimeSpan ELAPSEDTIME;
    private double ANCHORTIME;
    private double SETTIME;
    private double FPSTIME;
    
    [HideInInspector] public float realTimeMEAN;
    [HideInInspector] public float realTimeMEDIAN;
    [HideInInspector] public float realTimeWorst;
    
    public int[] frameCountEachSecond = new int[20]; //log every 10 seconds
    private int frameCounter = 0;
    private int frameSumForMean = 0;
    private int tenFrameIndexTail = -1;
    private int tenFrameIndexStart = 0;
    private bool once = false;

    private void Start()
    {
        realTimeWorst = int.MaxValue;
        ANCHORTIME = 0;
    }

    private void Update()
    {
        if (!once)
        {
            STARTTIME = System.DateTime.Now;
            once = true;
        }

        ELAPSEDTIME = System.DateTime.Now - STARTTIME;
        FPSTIME = ELAPSEDTIME.TotalSeconds - ANCHORTIME;

        if (FPSTIME >= 1)
        {
            ANCHORTIME = ELAPSEDTIME.TotalSeconds;

            //frame data
            frameCountEachSecond[tenFrameIndexStart%20] = frameCounter;

            if (tenFrameIndexStart > 9)
            {
                frameSumForMean += frameCounter;
                if (tenFrameIndexStart == 10)
                {
                    frameSumForMean -= frameCountEachSecond[0];
                }
                else
                {
                    frameSumForMean -= frameCountEachSecond[tenFrameIndexTail%20];
                }
            }
            else
            {
                frameSumForMean += frameCounter;
            }

            if (tenFrameIndexStart%10 == 0)
            {
                realTimeWorst = int.MaxValue;
            }
            
            realTimeMEAN = frameSumForMean / 10.0f;
            int half = (tenFrameIndexStart - tenFrameIndexTail) / 2;
            realTimeMEDIAN = frameCountEachSecond[(tenFrameIndexStart - half)%20]; //HOLY MACDONALD

            for (int i = tenFrameIndexTail + 1; i <= tenFrameIndexStart; i++) //o oh, performance??
            {
                if (realTimeWorst > frameCountEachSecond[(i) % 20])
                {
                    realTimeWorst = frameCountEachSecond[(i) % 20];
                }
            }
            
            //frame index handling
            tenFrameIndexStart++;
            if (tenFrameIndexStart > 10)
            {
                if (tenFrameIndexStart == 11)
                {
                    tenFrameIndexTail = 0;
                }
                tenFrameIndexTail++;
            }
            
            frameCounter = 0;
        }

        frameCounter++;
    }
}

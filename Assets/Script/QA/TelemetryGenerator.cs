using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class TelemetryGenerator : MonoBehaviour
{
    public static TelemetryGenerator instance;
    
    private int runCount;
    private string columns = "GameTime,MeanFPS,MedianFPS,WorstFPS," +
                             "ShipHealth,ShipPosition,ShipVelocity,Facing,ChainGunFireRate,DeployMissiles," +
                             "BossHealth,BossPosition,BossVelocity," +
                             "BossBackIsAttached, BossSpawningEnemyShipCooldown," +
                             "BossChargeGunIsAttached, BossChargingTimer,BossChargeShotCooldown," +
                             "BossStarGunIsAttached, BossStarGunCooldown\n";

    private System.DateTime STARTTIME;
    private System.TimeSpan ELAPSEDTIME;
    private double ANCHORTIME;
    private double SETTIME;
    private double FPSTIME;
    private double LOGTIME = 10; // 1 set = 10 seconds

    [HideInInspector] public float realTimeMEAN;
    [HideInInspector] public float realTimeMEDIAN;
    [HideInInspector] public int realTimeWorst;
    
    public int[] frameCountEachSecond = new int[20]; //log every 10 seconds
    private int frameCounter = 0;
    private int frameSumForMean = 0;
    private int tenFrameIndexTail = -1;
    private int tenFrameIndexStart = 0;
    private bool once = false;

    private static string path = "";

    public bool noTelemetry = false;
    
    private float logInterval = 30;
    private float logTimer = 0f;

    public Action Logging;
    
    #region degoobo
    private int shipHealth;
    private Vector3 shipPosition;
    private float shipVelocity;
    private Vector3 facing;
    private float chainGunFireRate;
    private bool missileDeployed;

    public void PlayerTelemetryData(int shipHealth, Vector3 shipPosition,
        float shipVelocity, Vector3 facing,
        float chainGunFireRate, bool missileDeployed)
    {
        this.shipHealth = shipHealth;
        this.shipPosition = shipPosition;
        this.shipVelocity = shipVelocity;
        this.facing = facing;
        this.chainGunFireRate = chainGunFireRate;
        this.missileDeployed = missileDeployed;
    }
    
    private int bossHealth;
    private Vector3 bossPosition;
    private float bossVelocity;
    private bool bossBackIsAttached;
    private float bossSpawningEnemyShipCooldown;
    private bool bossChargeGunIsAttached;
    private float bossChargingTimer;
    private float bossChargeShotCooldown;
    private bool bossStarGunIsAttached;
    private float bossStarGunCooldown;
    
    public void BossTelemetryData(int bossHealth, Vector3 bossPosition,
        float bossVelocity, bool bossBackIsAttached,float bossSpawningEnemyShipCooldown,
        bool bossChargeGunIsAttached, float bossChargingTimer, float bossChargeShotCooldown, 
        bool bossStarGunIsAttached, float bossStarGunCooldown)
    {
        this.bossHealth = bossHealth;
        this.bossPosition = bossPosition;
        this.bossVelocity = bossVelocity;
        this.bossBackIsAttached = bossBackIsAttached;
        this.bossSpawningEnemyShipCooldown = bossSpawningEnemyShipCooldown;
        this.bossChargeGunIsAttached = bossChargeGunIsAttached;
        this.bossChargingTimer = bossChargingTimer;
        this.bossChargeShotCooldown = bossChargeShotCooldown;
        this.bossStarGunIsAttached = bossStarGunIsAttached;
        this.bossStarGunCooldown = bossStarGunCooldown;
    }
    #endregion
    
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        
    }

    private void Start()
    {
        realTimeWorst = int.MaxValue;
        if (!PlayerPrefs.HasKey("testCount"))
        {
            PlayerPrefs.SetInt("testCount", 0);
        }
        runCount = PlayerPrefs.GetInt("testCount");
        InitCSV();
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

        logTimer += Time.unscaledDeltaTime;
        if (logTimer >= logInterval)
        {
            Log();
            logTimer = 0;
        }
    }

    public void InitCSV()
    {
        if (noTelemetry)
        {
            return;
        }
        string fileName = "test_" + runCount + ".csv";
        path = Path.Combine(Application.dataPath, fileName);

        if (!File.Exists(path))
        {
            File.WriteAllText(path, columns);
        }
    }

    public void Log()
    {
        if (noTelemetry)
        {
            return;
        }
        Logging?.Invoke();
        LogEvent(ELAPSEDTIME.TotalSeconds, realTimeMEAN, realTimeMEDIAN, realTimeWorst);
    }

    private void LogEvent(double gametime, float mean, double median, int worst)
    {
        if (worst == null || worst < 0 || worst >= int.MaxValue)
        {
            worst = 0;
        }
        
        string line = $"{gametime},{mean}, {median}, {worst}," +
                      $"{shipHealth}, {Vec3ToString(shipPosition)}, {shipVelocity}, {Vec3ToString(facing)}, {chainGunFireRate}, {missileDeployed}," +
                      $"{bossHealth}, {Vec3ToString(bossPosition)}, {bossVelocity}," +
                      $"{bossBackIsAttached}, {bossSpawningEnemyShipCooldown}, " +
                      $"{bossChargeGunIsAttached}, {bossChargingTimer}, {bossChargeShotCooldown}," +
                      $"{bossStarGunIsAttached}, {bossStarGunCooldown}" +
                      $"\n";
        
        File.AppendAllText(path, line + "\n");
    }

    private string Vec3ToString(Vector3 vec3)
    {
        string l = "";
        l += vec3.x.ToString("F2") + " | ";
        l += vec3.y.ToString("F2") + " | ";
        l += vec3.z.ToString("F2");
        return l;
    }
    
    public void SaveBeforeExitGame()
    {
        if (noTelemetry)
        {
            return;
        }
        PlayerPrefs.SetInt("testCount", runCount++);
        //GenerateCSV();
    }
}


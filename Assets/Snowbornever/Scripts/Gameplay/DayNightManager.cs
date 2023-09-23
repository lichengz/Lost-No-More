using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using HappyHarvest;


/// <summary>
/// The DayNightManager is the entry point to all the game system. It's execution order is set very low to make sure
/// its Awake function is called as early as possible so the instance if valid on other Scripts. 
/// </summary>
[DefaultExecutionOrder(-9999)]
public class DayNightManager : MonoBehaviour
{
    private static DayNightManager s_Instance;


#if UNITY_EDITOR
    //As our manager run first, it will also be destroyed first when the app will be exiting, which lead to s_Instance
    //to become null and so will trigger another instantiate in edit mode (as we dynamically instantiate the Manager)
    //so this is set to true when destroyed, so we do not reinstantiate a new one
    private static bool s_IsQuitting = false;
#endif
    public static DayNightManager Instance
    {
        get
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || s_IsQuitting)
                return null;

            if (s_Instance == null)
            {
                //in editor, we can start any scene to test, so we are not sure the game manager will have been
                //created by the first scene starting the game. So we load it manually. This check is useless in
                //player build as the 1st scene will have created the DayNightManager so it will always exists.
                Instantiate(Resources.Load<DayNightManager>("DayNightManager"));
            }
#endif
            return s_Instance;
        }
    }

    public TerrainManager Terrain { get; set; }
    public PlayerController Player { get; set; }
    public DayNightHandler DayNightHandler { get; set; }
    public WeatherSystem WeatherSystem { get; set; }
    public CinemachineVirtualCamera MainCamera { get; set; }
    public Tilemap WalkSurfaceTilemap { get; set; }

    public SceneData LoadedSceneData { get; set; }

    // Will return the ratio of time for the current day between 0 (00:00) and 1 (23:59).
    public float CurrentDayRatio => m_CurrentTimeOfTheDay / DayDurationInSeconds;
    
    [Header("Time settings")] [Min(1.0f)] public float DayDurationInSeconds;
    public float StartingTime = 0.0f;
    
    private bool m_IsTicking;

    private List<DayNightEvent> m_EventHandlers = new();
    private List<SpawnPoint> m_ActiveTransitions = new List<SpawnPoint>();

    private float m_CurrentTimeOfTheDay;

    private void Awake()
    {
        s_Instance = this;
        DontDestroyOnLoad(gameObject);

        m_IsTicking = true;

        m_CurrentTimeOfTheDay = StartingTime;

        //we need to ensure that we don't have a day length at 0, otherwise we will get stuck into infinite loop in update
        //(and a day with 0 length makes no sense)
        if (DayDurationInSeconds <= 0.0f)
        {
            DayDurationInSeconds = 1.0f;
            Debug.LogError(
                "The day length on the DayNightManager is set to 0, the length need to be set to a positive value");
        }
    }

    private void Start()
    {
        m_CurrentTimeOfTheDay = StartingTime;
    }

#if UNITY_EDITOR
    private void OnDestroy()
    {
        s_IsQuitting = true;
    }
#endif

    private void Update()
    {
        if (m_IsTicking)
        {
            float previousRatio = CurrentDayRatio;
            m_CurrentTimeOfTheDay += Time.deltaTime;

            while (m_CurrentTimeOfTheDay > DayDurationInSeconds)
                m_CurrentTimeOfTheDay -= DayDurationInSeconds;

            foreach (var handler in m_EventHandlers)
            {
                foreach (var evt in handler.Events)
                {
                    bool prev = evt.IsInRange(previousRatio);
                    bool current = evt.IsInRange(CurrentDayRatio);

                    if (prev && !current)
                    {
                        evt.OffEvent.Invoke();
                    }
                    else if (!prev && current)
                    {
                        evt.OnEvents.Invoke();
                    }
                }
            }

            if (DayNightHandler != null)
                DayNightHandler.Tick();
        }
    }

    public void Pause()
    {
        m_IsTicking = false;
        Player.ToggleControl(false);
    }

    public void Resume()
    {
        m_IsTicking = true;
        Player.ToggleControl(true);
    }

    public void RegisterSpawn(SpawnPoint spawn)
    {
        if (Player == null && spawn.SpawnIndex == 0)
        {
            //if we have no player, we need to create one
            Instantiate(Resources.Load<PlayerController>("Character"));
            spawn.SpawnHere();
        }

        m_ActiveTransitions.Add(spawn);
    }

    public void UnregisterSpawn(SpawnPoint spawn)
    {
        m_ActiveTransitions.Remove(spawn);
    }

    /// <summary>
    /// Will return the current time as a string in format of "xx:xx" 
    /// </summary>
    /// <returns></returns>
    public string CurrentTimeAsString()
    {
        return GetTimeAsString(CurrentDayRatio);
    }

    /// <summary>
    /// Return in the format "xx:xx" the given ration (between 0 and 1) of time
    /// </summary>
    /// <param name="ratio"></param>
    /// <returns></returns>
    public static string GetTimeAsString(float ratio)
    {
        var hour = GetHourFromRatio(ratio);
        var minute = GetMinuteFromRatio(ratio);

        return $"{hour}:{minute:00}";
    }


    public static int GetHourFromRatio(float ratio)
    {
        var time = ratio * 24.0f;
        var hour = Mathf.FloorToInt(time);

        return hour;
    }

    public static int GetMinuteFromRatio(float ratio)
    {
        var time = ratio * 24.0f;
        var minute = Mathf.FloorToInt((time - Mathf.FloorToInt(time)) * 60.0f);

        return minute;
    }

    public static void RegisterEventHandler(DayNightEvent handler)
    {
        foreach (var evt in handler.Events)
        {
            if (evt.IsInRange(DayNightManager.Instance.CurrentDayRatio))
            {
                evt.OnEvents.Invoke();
            }
            else
            {
                evt.OffEvent.Invoke();
            }
        }

        Instance.m_EventHandlers.Add(handler);
    }

    public static void RemoveEventHandler(DayNightEvent handler)
    {
        Instance?.m_EventHandlers.Remove(handler);
    }
}
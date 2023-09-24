using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinecraftClock : MonoBehaviour
{
    [SerializeField] private FloatEventChannelSO updateTimeChannel;

    [SerializeField] private TextMeshProUGUI time;
    private float hoursInDay;
    private float sunriseHour;
    private float sunsetHour;
    private float dayDuration;
    private float nightDuration;

    public RectTransform skyDome;

    float nightHoursToDegrees, dayHoursToDegrees;

    private void Awake()
    {
        updateTimeChannel.OnEventRaised += UpdateTime;
    }

    // void Start()
    // {
    //     nightHoursToDegrees = 180 / (hoursInDay * nightDuration);
    //     dayHoursToDegrees = 180 / (hoursInDay * (1 - nightDuration));
    //
    //     skyDome.rotation = Quaternion.Euler(0, 0, 90 + sunriseHour * nightHoursToDegrees);
    // }
    //
    // void UpdateTime(float hour)
    // {
    //     if (((hour < sunriseHour || hour > sunsetHour) && sunriseHour < sunsetHour) ||
    //         ((hour < sunriseHour && hour > sunsetHour) && sunriseHour > sunsetHour))
    //     {
    //         skyDome.Rotate(0, 0, -Time.deltaTime * TimeManager.hoursInDay * nightHoursToDegrees / dayDuration);
    //     }
    //     else
    //     {
    //         skyDome.Rotate(0, 0, -Time.deltaTime * TimeManager.hoursInDay * dayHoursToDegrees / dayDuration);
    //     }
    // }
    
    private void UpdateTime(float ratio)
    {
        var hour = GetHourFromRatio(ratio);
        var minute = GetMinuteFromRatio(ratio);

        time.text = $"{hour}:{minute:00}";
    }
    
    private int GetHourFromRatio(float ratio)
    {
        var time = ratio * 24.0f;
        var hour = Mathf.FloorToInt(time);

        return hour;
    }

    private int GetMinuteFromRatio(float ratio)
    {
        var time = ratio * 24.0f;
        var minute = Mathf.FloorToInt((time - Mathf.FloorToInt(time)) * 60.0f);

        return minute;
    }
}
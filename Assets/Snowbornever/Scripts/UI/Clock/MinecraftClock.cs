using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinecraftClock : MonoBehaviour
{
    [SerializeField] private FloatEventChannelSO updateTimeChannel;

    [SerializeField] private TextMeshProUGUI time;

    public RectTransform skyDome;
    
    private void Awake()
    {
        updateTimeChannel.OnEventRaised += UpdateTime;
    }

    private void UpdateTime(float ratio)
    {
        var hour = GetHourFromRatio(ratio);
        var minute = GetMinuteFromRatio(ratio);

        time.text = $"{hour}:{minute:00}";
        
        skyDome.rotation = Quaternion.Euler(0, 0, 180 - ratio * 360);
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
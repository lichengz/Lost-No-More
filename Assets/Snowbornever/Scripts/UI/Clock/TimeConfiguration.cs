using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TimeConfiguration", menuName = "ScriptableObjects/TimeConfiguration")]
public class TimeConfiguration : ScriptableObject
{
    public int hoursInDay = 24;
    public int minutesInHour = 60;
    
}

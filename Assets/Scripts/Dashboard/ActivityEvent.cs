using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityEvent
{
    public string activity_ptr_id{ get; }
    public Activity activity{ get; }
    public DateTime start_date { get; }
    public DateTime end_date { get; }
    public TimeSpan start_time{ get; }
    public TimeSpan end_time { get; }
    public bool is_recurring{ get; }

    public ActivityEvent(string ptr, DateTime sd, DateTime ed, TimeSpan st, TimeSpan et, bool is_recurring)
    {
        start_date = sd;
        end_date = ed;
        start_time = st;
        end_time = et;
        activity_ptr_id = ptr;
        
        
        activity = Database.Instance.GetActivity(activity_ptr_id);

        this.is_recurring = is_recurring;
    }
    public ActivityEvent()
    {
    }
    public void Initialize()
    {
        //Database.Instance.
    }
    public DateTime GetStartDate()
    {
        return start_date;
    }
    public bool isRecurring()
    {
        return is_recurring;
    }
}

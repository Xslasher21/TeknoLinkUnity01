using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScheduleUI : MonoBehaviour
{
    public TextMeshProUGUI event_name;
    public TextMeshProUGUI time;
}
public class Schedule
{
    public string ID { get; }
    public ActivityEvent activityEvent { get; }
    public string student_id_id { get; }

    public Schedule(string id, string event_schedule_id_id, string student_id_id)
    {
        ID = id;
        this.student_id_id = student_id_id;

        activityEvent = Database.Instance.GetActivityEvent(event_schedule_id_id);

    }
    public Schedule()
    {
        ID = null;
        activityEvent = null;
        student_id_id = null;
    }
}

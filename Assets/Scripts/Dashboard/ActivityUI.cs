using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ActivityUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Name = null;
    [SerializeField] TextMeshProUGUI Date = null;
    [SerializeField] TextMeshProUGUI Sched = null;
    [SerializeField] TextMeshProUGUI Status = null;
    [SerializeField] Button button = null;
    public GameObject panel = null;
    public Activity activity;

    public void SetActivity(string name, string date, string sched, string status)
    {
        if (Name != null) Name.text = name;
        if (Date != null) Date.text = date;
        if (Sched != null) Sched.text = sched;
        if (Status != null) Status.text = status;
    }

    public Button getButton()
    {
        return button;
    }

    public void goToActivity()
    {
        if (panel != null)
        {
            panel.SetActive(true);
            panel.GetComponent<Animator>().SetTrigger("comeRight");
            ActivityUI u = panel.GetComponent<ActivityUI>();
            u.SetActivity(activity.title, activity.valid_until_date.ToString(), activity.creator_community_id_id, activity.description);
        }
    }
}
public class Activity
{
    public string ID { get; }
    public string title { get; }
    public string description { get; }
    public DateTime valid_until_date { get; }
    public string activity_type { get; }
    public string chat_room_id_id { get; }
    public string parent_activity_id_id { get; }
    public string status_id_id { get; }
    public string creator_community_id_id { get; }
    public string status_admin_id_id { get; }

    public Activity(string ID, string title, string description, DateTime valid_until_date, string activity_type, string chat_room_id_id, string parent_activity_id_id, string status_id_id, string creator_community_id_id, string status_admin_id_id)
    {
        this.ID = ID;
        this.title = title;
        this.description = description;
        this.valid_until_date = valid_until_date;
        this.activity_type = activity_type;
        this.chat_room_id_id = chat_room_id_id;
        this.parent_activity_id_id = parent_activity_id_id;
        this.status_id_id = status_id_id;
        this.creator_community_id_id = creator_community_id_id;
        this.status_admin_id_id = status_admin_id_id;
    }
    public Activity()
    {

    }
}

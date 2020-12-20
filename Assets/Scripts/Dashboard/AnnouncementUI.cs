using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnnouncementUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Title = null;
    [SerializeField] TextMeshProUGUI Date = null;
    [SerializeField] TextMeshProUGUI Owner = null;
    [SerializeField] TextMeshProUGUI Description = null;
    public GameObject panel = null;
    //[SerializeField] Button button = null;
    public Activity activity;


    public void SetAnnouncement(string title, string date, string owner, string description)
    {
        if(Title != null) Title.text = title;
        if (Date != null) Date.text = date;
        if (Owner != null)
        {
            Community c = Database.Instance.GetCommunity(owner);

            Owner.text = c.name;
        }
        if (Description != null) Description.text = description;
    }
    public void goToAnnouncement()
    {
        if(panel != null)
        {
            panel.SetActive(true);
            panel.GetComponent<Animator>().SetTrigger("comeRight");
            AnnouncementUI u = panel.GetComponent<AnnouncementUI>();
            u.SetAnnouncement(activity.title, activity.valid_until_date.ToString(),activity.creator_community_id_id,activity.description);
        }
    }
}

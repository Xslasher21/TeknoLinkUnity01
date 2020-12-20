using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Notification
{
    public string ID { get; }
    public DateTime send_datetime { get; }
    public string isRead { get; }
    public User sender_user { get; }
    public string receiver_user_id_id { get; }
    public string type_id_id { get; }
    public string text { get; }
    public Community community { get; }

    public Notification(string id, DateTime send_datetime, string isRead, string sender_user_id_id, string receiver_user_id_id, string type_id_id, string text)
    {
        this.ID = id;
        this.send_datetime = send_datetime;
        this.isRead = isRead;
        this.receiver_user_id_id = receiver_user_id_id;
        this.type_id_id = type_id_id;
        this.text = text;

        sender_user = Database.Instance.GetUser(sender_user_id_id);
        if(sender_user.user_type == "Community")
        {
            community = Database.Instance.GetCommunity(sender_user_id_id);
        }
        else
        {
            community = null;
        }
    }
}
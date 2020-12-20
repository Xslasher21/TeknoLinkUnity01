using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CommunityUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Name = null;
    [SerializeField] TextMeshProUGUI Description = null;
    [SerializeField] TextMeshProUGUI Location = null;
    [SerializeField] Button button = null;
    public GameObject panel = null;
    public Community community;

    public void SetCommunity(string name, string desc, string loc)
    {
        if (Name != null) Name.text = name;
        if (Description != null) Description.text = desc;
        if (Location != null) Location.text = loc;
    }

    public Button getButton()
    {
        return button;
    }

    public void goToCommunity()
    {
        if (panel != null)
        {
            panel.SetActive(true);
            panel.GetComponent<Animator>().SetTrigger("comeRight");
            CommunityUI u = panel.GetComponent<CommunityUI>();
            u.SetCommunity(community.name, community.description, community.location);
        }
    }
}

public class Community
{
    public string user_ptr_id { get; }
    //public DateTime date_created { get; }
    public string name { get; }
    public string isAdmin { get; }
    public string isOffice { get; }
    public string description { get; }
    public string location { get; }
    public string isDeleted { get; }

    public Community(string user_ptr_id, string name, string isAdmin, string isOffice, string description, string location, string isDeleted)
    {
        this.user_ptr_id = user_ptr_id;
        //this.date_created = date_created;
        this.name = name;
        this.isAdmin = isAdmin;
        this.isOffice = isOffice;
        this.description = description;
        this.location = location;
        this.isDeleted = isDeleted;
    }
}
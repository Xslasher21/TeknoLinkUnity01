using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.Text;
using System.ComponentModel;
using System.Net.Http;

public class DashboardUI : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject notif_prefab;
    public GameObject notif_parent;
    public Image[] profilePics;

    public TextMeshProUGUI profileName, userInfo, address;


    private bool getInfo = false;
    void Start()
    {
        AddNotif();
        Database.Instance.GetPic("User",PlayerPrefs.GetString(Database.uID),profilePics);
        Database.Instance.GetUserInfo(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!getInfo && Database.Instance.HasBasicInfo == true)
        {
            profileName.text = Database.Instance.GetUserBasicInfo("Name:");
            userInfo.text = Database.Instance.GetUserBasicInfo("Course:");
            userInfo.text = userInfo.text + " - " + Database.Instance.GetUserBasicInfo("Year:");
            userInfo.text = userInfo.text + "\n" + Database.Instance.GetUserBasicInfo("Contact:");
            address.text = Database.Instance.GetUserBasicInfo("Address:");
            getInfo = true;
        }
    }

    private void AddNotif()
    {
        //Test
        GameObject gb = Instantiate(notif_prefab, notif_parent.transform);
        gb.GetComponent<Notification_btn>().notif_logo.color = Color.green;
    }


    public void changePanel(int position)
    {
        foreach (GameObject i in panels)
        {
            i.SetActive(false);
        }
        panels[position].SetActive(true);

    }
}

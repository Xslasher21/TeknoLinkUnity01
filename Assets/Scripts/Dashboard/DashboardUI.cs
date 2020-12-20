using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DashboardUI : MonoBehaviour
{
    [SerializeField] private GameObject[] panels = null;
    [SerializeField] private GameObject[] extraPanels = null;
    [SerializeField] private GameObject notif_prefab = null;
    [SerializeField] private GameObject notif_parent = null;
    [SerializeField] private GameObject announce_prefab = null;
    [SerializeField] private GameObject announce_parent = null;
    [SerializeField] private GameObject announce_parent1 = null;
    [SerializeField] private GameObject activity_prefab = null;
    [SerializeField] private GameObject activity_parent = null;
    [SerializeField] private GameObject community_prefab = null;
    [SerializeField] private GameObject community_parent = null;
    [SerializeField] private GameObject communityList_parent = null;
    [SerializeField] private Image[] profilePics = null;
    [SerializeField] private TextMeshProUGUI MainTopText = null;

    [SerializeField]
    private TextMeshProUGUI profileName = null, userInfo = null, address = null;

    private int dashboardPos = 2;

    private Stack<GameObject> onQueue;


    void Start()
    {
        onQueue = new Stack<GameObject>();
        //Database.Instance.GetPic("User",PlayerPrefs.GetString(Database.uID),profilePics);

        Database.Instance.GetStudent();
        Database.Instance.GetNotification(Database.Instance.student.user_id);
        Database.Instance.GetSchedule(Database.Instance.student.user_id);
        Database.Instance.GetAnnouncement();
        Database.Instance.GetActivities();
        Database.Instance.GetCommunities();
        Database.Instance.GetCommunityList();

        foreach (Activity item in Database.Instance.announcements)
        {
            GameObject gb = Instantiate(announce_prefab, announce_parent.transform);
            AnnouncementUI announce = gb.GetComponent<AnnouncementUI>();

            announce.SetAnnouncement(item.title, null, null, item.description);
            announce.activity = item;
            announce.panel = extraPanels[0];
            gb.GetComponent<Button>().onClick.AddListener(() => gotoAnnouncement());
            gb.GetComponent<Button>().onClick.AddListener(() => announce.goToAnnouncement());


            GameObject gb1 = Instantiate(announce_prefab, announce_parent1.transform);
            announce = gb1.GetComponent<AnnouncementUI>();


            announce.SetAnnouncement(item.title, null, null, item.description);
            announce.activity = item;
            announce.panel = extraPanels[0];
            gb1.GetComponent<Button>().onClick.AddListener(() => gotoAnnouncement());
            gb1.GetComponent<Button>().onClick.AddListener(() => announce.goToAnnouncement());


        }
        foreach (Activity item in Database.Instance.activities)
        {
            GameObject gb = Instantiate(activity_prefab, activity_parent.transform);
            ActivityUI activity = gb.GetComponent<ActivityUI>();


            activity.SetActivity(item.title, item.valid_until_date.Date.ToString("MMM/d/yyyy"), item.valid_until_date.Date.ToString("HH:mm"), item.status_id_id);
            activity.activity = item;
            activity.panel = extraPanels[1];
            activity.getButton().onClick.AddListener(() => gotoActivity());
            activity.getButton().onClick.AddListener(() => activity.goToActivity());
        }
        foreach (Community item in Database.Instance.communities)
        {
            GameObject gb = Instantiate(community_prefab, community_parent.transform);
            CommunityUI community = gb.GetComponent<CommunityUI>();


            community.SetCommunity(item.name, item.description, item.location);
            community.community = item;
            community.panel = extraPanels[6];
            community.getButton().onClick.AddListener(() => gotoCommunity());
            community.getButton().onClick.AddListener(() => community.goToCommunity());
        }
        foreach (Community item in Database.Instance.communityList)
        {
            GameObject gb = Instantiate(community_prefab, communityList_parent.transform);
            CommunityUI community = gb.GetComponent<CommunityUI>();


            community.SetCommunity(item.name, item.description, item.location);
            community.community = item;
            community.panel = extraPanels[6];
            community.getButton().onClick.AddListener(() => gotoCommunity());
            community.getButton().onClick.AddListener(() => community.goToCommunity());
        }

        NotifInitialize();



        //profileName.text = Database.Instance.GetUserBasicInfo("first_name:", "Student",0);
        profileName.text = Database.Instance.student.first_name;
        profileName.text += " " + Database.Instance.student.middle_name;
        profileName.text += " " + Database.Instance.student.last_name;


        //userInfo.text = Database.Instance.GetUserBasicInfo("description:", "Student_1",0);
        userInfo.text = Database.Instance.student.department.Description;
        //userInfo.text = userInfo.text + " - " + Database.Instance.GetUserBasicInfo("Year:");
        //userInfo.text = userInfo.text + "\n" + Database.Instance.GetUserBasicInfo("Contact:");

        address.text = Database.Instance.student.street_address;
        address.text += ", " + Database.Instance.student.brgy_address;
        address.text += ", " + Database.Instance.student.city_address;
        address.text += ", " + Database.Instance.student.province_address;
        address.text += ", " + Database.Instance.student.zip_address;
        address.text += ", " + Database.Instance.student.country_address;




    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Escape();

            // Guide for buttons
            //extraPanels[0].GetComponent<Button>().onClick.AddListener(() => AddOnQueue(1));
        }

    }
    public void Escape()
    {
        if (onQueue.Count > 0)
        {
            GameObject gb = onQueue.Peek();
            gb.GetComponent<Animator>().SetTrigger("goRight");

            onQueue.Pop();
        }
        else
        {

        }
    }

    public void gotoAnnouncement()
    {
        extraPanels[0].SetActive(true);
        extraPanels[0].GetComponent<Animator>().SetTrigger("comeRight");
        AddOnQueue(0);
    }
    public void gotoActivity()
    {
        extraPanels[1].SetActive(true);
        extraPanels[1].GetComponent<Animator>().SetTrigger("comeRight");
        AddOnQueue(1);
    }
    public void gotoMessage()
    {
        extraPanels[2].SetActive(true);
        extraPanels[2].GetComponent<Animator>().SetTrigger("comeRight");
        AddOnQueue(2);
    }
    public void gotoCommunity()
    {
        extraPanels[6].SetActive(true);
        extraPanels[6].GetComponent<Animator>().SetTrigger("comeRight");
        AddOnQueue(6);
    }
    public void gotoCommunityList()
    {
        extraPanels[7].SetActive(true);
        extraPanels[7].GetComponent<Animator>().SetTrigger("comeRight");
        AddOnQueue(7);
    }


    public void AddOnQueue(int obj)
    {
        onQueue.Push(extraPanels[obj]);
    }

    public void NotifInitialize()
    {
        foreach (Notification item in Database.Instance.notifications)
        {
            GameObject gb = Instantiate(notif_prefab, notif_parent.transform);
            Notification_btn notif = gb.GetComponent<Notification_btn>();
            notif.notif_logo.color = Color.blue;
            notif.notif_sender.text = item.community.name;
            notif.notif_message.text = item.text;
            notif.time_stamp.text = item.send_datetime.ToString("t");
        }
    }


    public void changePanel(int position)
    {
        foreach (GameObject i in extraPanels)
            i.SetActive(false);

        while (onQueue.Count > 0)
        {
            onQueue.Pop();
        }

        panels[position].SetActive(true);

        if (dashboardPos < position)
        {
            panels[dashboardPos].GetComponent<Animator>().SetTrigger("goLeft");
            panels[position].GetComponent<Animator>().SetTrigger("comeRight");

        }
        else if (dashboardPos > position)
        {
            panels[dashboardPos].GetComponent<Animator>().SetTrigger("goRight");
            panels[position].GetComponent<Animator>().SetTrigger("comeLeft");

        }
        dashboardPos = position;

        switch (position)
        {
            case 0: MainTopText.text = "Chat"; break;
            case 1: MainTopText.text = "Activity"; break;
            case 2: MainTopText.text = "Home"; break;
            case 3: MainTopText.text = "Notification"; break;
            case 4: MainTopText.text = "Calendar"; break;
        }
    }
}

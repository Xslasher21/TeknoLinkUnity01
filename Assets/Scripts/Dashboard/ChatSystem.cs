using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ChatSystem : MonoBehaviour
{
    [SerializeField]
    List<GameObject> messageList = new List<GameObject>();
    List<GameObject> ChatRoomList = new List<GameObject>();
    [SerializeField] DashboardUI Dashboard = null;

    [SerializeField] GameObject message_Prefab = null;
    [SerializeField] GameObject messageLoc = null;
    [SerializeField] GameObject chatRoom_Prefab = null;
    [SerializeField] GameObject chatRoomLoc = null;

    [SerializeField] GameObject MessagePanel = null;
    [SerializeField] TMP_InputField messageField = null;


    bool HasChatRoom = false,HasMessages = false;

    // Start is called before the first frame update
    void Start()
    {
        Database.Instance.GetChatRooms();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SendMessageToChat("Heyoo", "date time", "test name",false);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SendMessageToChat("oh yeehh", "date time", "test name", true);
        }
        if (!HasChatRoom && Database.Instance.HasChatRoom)
        {
            int i = 0, len = Database.Instance.GetRoomLen() - 1;

            while (i < len)
            {
                AddChatRooms(i++);
            }
            HasChatRoom = true;
        }
        if (!HasMessages && Database.Instance.HasMessages)
        {
            int i = 0, len = Database.Instance.GetMessagesLen() - 1;

            while (i < len)
            {
                string text = Database.Instance.GetUserBasicInfo("content:", "Messages", i);
                string sender = Database.Instance.GetUserBasicInfo("sender_user_id_id:", "Messages", i);
                string date = Database.Instance.GetUserBasicInfo("send_datetime:", "Messages", i);
                SendMessageToChat(text ,date.Substring(0,date.Length - 7), sender, false);
                
                Debug.Log(System.DateTime.UtcNow);

                //Debug.Log(System.DateTime.Now);
                i++;
            }
            HasMessages = true;
        }
    }
    public void SendMessage()
    {
        if(messageField.text.Length > 0)
            SendMessageToChat(messageField.text, DateTime.Now.ToString("yyyy-MMM-dd") , PlayerPrefs.GetString("userID"), true);
        messageField.text = "";

    }



    public void SendMessageToChat(string text,string date,string sender,bool newMessage)
    {
        GameObject gb = Instantiate(message_Prefab, messageLoc.transform);
        Message m = gb.GetComponent<Message>();
        m.init_message(text, date, sender);
        if (newMessage)
        {
            gb.transform.SetSiblingIndex(0);
        }

        messageList.Add(gb);
    }

    private void AddChatRooms(int index)
    {
        GameObject gb = Instantiate(chatRoom_Prefab, chatRoomLoc.transform);
        ChatRoom c = gb.GetComponent<ChatRoom>();
        int i = System.Convert.ToInt32(Database.Instance.GetUserBasicInfo("room_id_id:", "ChatRooms", index));

        c.init_chatRoom(i);
        c.btn.onClick.AddListener(() => OpenChatRoom(i));

        ChatRoomList.Add(gb);
    }
    public void OpenChatRoom(int i)
    {
        Database.Instance.HasMessages = false;
        HasMessages = false;
        foreach (GameObject item in messageList)
        {
            Destroy(item);
        }
        messageList.Clear();

        Database.Instance.GetMessages(i);
        //MessagePanel.SetActive(true);
        //MessagePanel.GetComponent<Animator>().SetTrigger("comeRight");
        Dashboard.gotoMessage();

    }
}

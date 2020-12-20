using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Message : MonoBehaviour
{
    string text = null;
    string date = null;
    string sender_name = null;
    string my_id = null;

    public TextMeshProUGUI textUI;
    public TextMeshProUGUI dateUI;
    public TextMeshProUGUI sender_nameUI;

    void Start()
    {

    }
    public void init_message(string t, string d, string s)
    {
        textUI.text = text = t;
        dateUI.text = date = d;
        sender_nameUI.text = sender_name = s;
    }
    public Message()
    {
        text = null;
        date = null;
        sender_name = null;
    }
    public Message(string t, string d, string s)
    {
        text = t;
        date = d;
        sender_name = s;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

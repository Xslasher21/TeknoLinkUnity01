using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatRoom : MonoBehaviour
{
    private int id=0;

    [SerializeField] TextMeshProUGUI room_id = null;
    public Button btn = null;
    public void init_chatRoom(int id)
    {
        this.id = id;
        room_id.text = id.ToString();
    }
}

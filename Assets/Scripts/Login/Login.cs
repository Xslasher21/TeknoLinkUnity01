using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using UnityEngine.UI;

public class Login : MonoBehaviour
{

    public InputField userID, password;

    private string user;
    // Start is called before the first frame update
    private void Start()
    {
        user = PlayerPrefs.GetString(Database.uID, "null");
        if (user != null)
        {
            userID.text = user;
        }

    }

    public void GetUsers()
    {
        Database.Instance.FindUser(userID.text, password.text);
    }
}

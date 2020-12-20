using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{

    [SerializeField] InputField userID = null, password = null;
    [SerializeField] TextMeshProUGUI errorText = null;
    [SerializeField] GameObject errorPanel = null;
    [SerializeField] GameObject loadingPanel = null;

    private string user;
    // Start is called before the first frame update
    private void Start()
    {
        user = PlayerPrefs.GetString(Database.uID, "null");
        if (user != null)
        {
            userID.text = user;
        }
        Database.Instance.errors = null;

    }
    private void Update()
    {
        if (Database.Instance.errors != null)
        {
            errorText.text = Database.Instance.errors;
            errorPanel.SetActive(true);
            loadingPanel.SetActive(false);
        }
    }

    public void GetUsers()
    {
        if (checkUserID() && password.text.Length > 5)
        {
            loadingPanel.SetActive(true);
            Database.Instance.LoginUser(userID.text, password.text);
        }
        else
        {
            errorText.text = "Invalid user ID and/or password.";
            errorPanel.SetActive(true);
        }
    }
    public void ResetError()
    {
        Database.Instance.errors = null;
    }
    private bool checkUserID()
    {
        bool flag = true;
        if(userID.text.Length > 50)
        {
            flag = false;
        }
        return flag;
    }
}

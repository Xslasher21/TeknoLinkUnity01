using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Database : MonoBehaviour
{
    public static Database Instance { get; set; }
    public const string uID = "userID";
    public bool HasBasicInfo = false;

    private string[] usersData;
    private string usersDataProfile = null;

    private string URL1 = "http://192.168.0.113/test/userFind.php";
    private string URL2 = "http://192.168.0.113/test/userSelect.php";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FindUser(string userID, string password)
    {
        StartCoroutine(IFindUser(userID, password));
    }

    IEnumerator IFindUser(string userID, string password)
    {
        // byte[] bytes = System.Text.Encoding.ASCII.GetBytes(b);

        //UploadHandlerRaw uH = new UploadHandlerRaw(bytes);
        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        List<IMultipartFormSection> forms = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("whereField", "User"),
            new MultipartFormDataSection("whereCondition", userID),
            new MultipartFormDataSection("whereField1", "Password"),
            new MultipartFormDataSection("whereCondition1", password)
        };
        UnityWebRequest users = UnityWebRequest.Post(URL1, forms);

        //users.uploadHandler = uH;
        users.downloadHandler = dH;
        yield return users.SendWebRequest();

        if (users.isNetworkError || users.isHttpError)
        {
            Debug.LogError(users.error);
        }
        else
        {
            if (users.downloadHandler != null)
            {
                string userDataString = users.downloadHandler.text.Replace(" ", "");

                Debug.Log(userDataString);
                if (userDataString == "true")
                {
                    Debug.Log("Login Successful");
                    PlayerPrefs.SetString(uID, userID);
                    SceneManager.LoadSceneAsync("Dashboard");
                }
                /*usersData = userDataString.Split(';');
                int len = usersData.Length - 1;
                for( int i = 0; i < len; i++)
                {
                    string u = GetValueData(usersData[i], "UserID:").Replace(" ", "");
                    string p = GetValueData(usersData[i], "Password:").Replace(" ", "");
                    if (u.Equals(userID.text) && p.Equals(password.text))
                    {
                        Debug.Log("Login Successful");
                        SceneManager.LoadSceneAsync("Dashboard");
                    }
                }*/
            }
        }
    }
    public void GetUserInfo()
    {
        StartCoroutine(IGetUserInfo());
    }
    IEnumerator IGetUserInfo()
    {
        //UploadHandlerRaw uH = new UploadHandlerRaw(bytes);
        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        List<IMultipartFormSection> forms = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("whereField", "User"),
            new MultipartFormDataSection("whereCondition", PlayerPrefs.GetString(uID))
        };

        UnityWebRequest users = UnityWebRequest.Post(URL2, forms);

        //users.uploadHandler = uH;
        users.downloadHandler = dH;
        yield return users.SendWebRequest();

        if (users.isNetworkError || users.isHttpError)
        {
            Debug.LogError(users.error);
        }
        else
        {
            if (users.downloadHandler != null)
            {
                string userDataString = users.downloadHandler.text;

                usersDataProfile = userDataString;
                HasBasicInfo = true;
            }
        }

    }
    public void GetPic(string whereField, string whereCondition, Image[] pictures)
    {
        StartCoroutine(IGetPic( whereField, whereCondition, pictures));
    }

    IEnumerator IGetPic(string whereField, string whereCondition,Image[] pictures)
    {
        string URL1 = "http://192.168.0.113/test/userPic.php";
        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        List<IMultipartFormSection> forms = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("whereField", whereField),
            new MultipartFormDataSection("whereCondition", whereCondition),
        };
        UnityWebRequest users = UnityWebRequest.Post(URL1, forms);

        //users.uploadHandler = uH;
        users.downloadHandler = dH;
        yield return users.SendWebRequest();

        if (users.isNetworkError || users.isHttpError)
        {
            Debug.LogError(users.error);
        }
        else
        {
            if (users.downloadHandler != null && users.downloadHandler.text != "false" && users.downloadHandler.text != "")
            {
                Texture2D tex = new Texture2D(256, 256, TextureFormat.RGBA32, false);
                byte[] bytes = Convert.FromBase64String(users.downloadHandler.text);
                tex.LoadImage(bytes);
                tex.Apply();
                Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height),Vector2.zero);
                foreach(Image i in pictures)
                    i.sprite = s;
            }
        }
    }
    private string GetValueData(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
        {
            value = value.Remove(value.IndexOf("|"));
        }
        return value;
    }
    //Only for basic info
    public string GetUserBasicInfo(string index)
    {
        if(index != null && index != "UserID:" && index != "Password:")
        {
            string value = usersDataProfile.Substring(usersDataProfile.IndexOf(index) + index.Length);
            if (value.Contains("|"))
            {
                value = value.Remove(value.IndexOf("|"));
            }
            return value;
        }
        else
        {
            return null;
        }
    }
}

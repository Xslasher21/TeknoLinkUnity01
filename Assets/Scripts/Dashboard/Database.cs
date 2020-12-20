using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
public class Database : MonoBehaviour
{
    public static Database Instance { get; set; }

    //for local storage
    public const string uID = "userID";
    
    public bool HasChatRoom = false;
    public bool HasMessages = false;

    public string errors = null;

    private string[] chatRooms = null;
    private string[] messages = null;
    //private string[] schedules = null;

    public List<Student> students = new List<Student>();
    public Student student = null;

    public List<Notification> notifications = new List<Notification>();

    public List<Schedule> schedules = new List<Schedule>();
    public List<Activity> announcements = new List<Activity>();
    public List<Activity> activities = new List<Activity>();
    public List<Activity> events = new List<Activity>();
    public List<Community> communities = new List<Community>();
    public List<Community> communityList = new List<Community>();




    //private readonly string URL_userFind = "http://localhost/test/userFind.php";

    //private readonly string URL_userSelect = "http://localhost/test/userSelect.php";
    //private readonly string URL_deptSelect = "http://localhost/test/deptSelect.php";
   // private readonly string URL_studentSelect = "http://localhost/test/studentSelect.php";
    //private readonly string URL_studentSchedID = "http://localhost/test/studentSchedID.php";
    //private readonly string URL_studentEventSelect = "http://localhost/test/studentEventSelect.php";
    //private readonly string URL_notifSelect = "http://localhost/test/notifSelect.php";
   // private readonly string URL_comSelect = "http://localhost/test/comSelect.php";
    private readonly string URL_roomPartSelect = "http://localhost/test/roomPartSelect.php";
    private readonly string URL_messageSelect = "http://localhost/test/messageSelect.php";


    private string URL4 = "http://localhost/test/userPic.php";
    private readonly string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=test2;";

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


    public void LoginUser(string userID, string password)
    {
        string query = "SELECT user_id,password FROM user where user_id = '" + userID + "' AND password = '"+ password + "' and user_type='Student';";

        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {
                bool check = false;
                while (reader.Read())
                {
                    // As our database, the array will contain : ID 0, FIRST_NAME 1,LAST_NAME 2, ADDRESS 3
                    // Do something with every received database ROW
                    check = true;
                }
                if (check)
                {
                    Debug.Log("Login Successful");
                    PlayerPrefs.SetString(uID, userID);
                    SceneManager.LoadSceneAsync("Dashboard");
                }
                else 
                {

                    Debug.Log("Login something happen");
                }
            }
            else
            {
                errors = "User doesn't exist";
            }

            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    IEnumerator IChatRooms()
    {
        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        List<IMultipartFormSection> forms = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("whereField", "user_id_id"),
            new MultipartFormDataSection("whereCondition", PlayerPrefs.GetString(uID))
        };

        UnityWebRequest users = UnityWebRequest.Post(URL_roomPartSelect, forms);

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
                chatRooms = users.downloadHandler.text.Split('`');
                HasChatRoom = true;

            }
        }
    }
    public void GetChatRooms()
    {
        StartCoroutine(IChatRooms());
    }
    public int GetRoomLen()
    {
        return chatRooms.Length;
    }
    IEnumerator IMessages(int id)
    {
        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        List<IMultipartFormSection> forms = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("whereField", "room_owner_id_id"),
            new MultipartFormDataSection("whereCondition", id.ToString())
        };

        UnityWebRequest users = UnityWebRequest.Post(URL_messageSelect, forms);

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
                messages = users.downloadHandler.text.Split('`');
                HasMessages = true;
            }
        }
    }
    public int GetMessagesLen()
    {
        return messages.Length;
    }
    public void GetMessages(int id)
    {
        StartCoroutine(IMessages(id));
    }


    #region Get Pic
    public void GetPic(string whereField, string whereCondition, Image[] pictures)
    {
        StartCoroutine(IGetPic( whereField, whereCondition, pictures));
    }

    IEnumerator IGetPic(string whereField, string whereCondition,Image[] pictures)
    {
        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();

        List<IMultipartFormSection> forms = new List<IMultipartFormSection>
        {
            new MultipartFormDataSection("whereField", whereField),
            new MultipartFormDataSection("whereCondition", whereCondition),
        };
        UnityWebRequest users = UnityWebRequest.Post(URL4, forms);

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
    #endregion

    #region Get Basic Info

    //Only for basic info
    public string GetUserBasicInfo(string index, string profile, int i)
    {
        string value = null;
        if (index != null)
        {
            switch (profile)
            {
                case "ChatRooms":
                    value = chatRooms[i].Substring(chatRooms[i].IndexOf(index) + index.Length);
                    if (value.Contains("|"))
                    {
                        value = value.Remove(value.IndexOf("|"));
                    }
                    break;
                case "Messages":
                    value = messages[i].Substring(messages[i].IndexOf(index) + index.Length);
                    if (value.Contains("|"))
                    {
                        value = value.Remove(value.IndexOf("|"));
                    }
                    break;
                default:
                    break;
            }
        }

        return value;
    }
    #endregion


    public void GetStudent()
    {
        string query = "SELECT * FROM student where user_ptr_id = '" + PlayerPrefs.GetString(uID) + "';";

        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    // As our database, the array will contain : ID 0, FIRST_NAME 1,LAST_NAME 2, ADDRESS 3
                    // Do something with every received database ROW
                    //User user = new User(reader.GetString(0), reader.GetString(1), reader.GetString(2));
                    Student s = new Student(reader.GetString("user_ptr_id"), reader.GetString("first_name"), reader.GetString("middle_name"), reader.GetString("last_name"), reader.GetString("street_address"), reader.GetString("brgy_address"), reader.GetString("city_address"), reader.GetString("province_address"), reader.GetString("zip_address"), reader.GetString("country_address"), reader.GetString("department_id_id"));
                    students.Add(s);
                    student = s;

                }
            }
            else
            {
                //text.text = "No rows found.";
            }

            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public Department GetDepartment(string id)
    {
        string query = "SELECT * FROM department where id = '" + id + "';";
        Department d = null;

        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    d = new Department(reader.GetString("id"), reader.GetString("name"), reader.GetString("description"), reader.GetString("college_id_id"));
                }
            }
            else
            {
                //text.text = "No rows found.";
            }

            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return d;
    }
    public College GetCollege(string id)
    {
        string query = "SELECT * FROM college where id = '" + id + "';";
        College c = null;

        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    // As our database, the array will contain : ID 0, FIRST_NAME 1,LAST_NAME 2, ADDRESS 3
                    // Do something with every received database ROW
                    //User user = new User(reader.GetString(0), reader.GetString(1), reader.GetString(2));
                    //Student s = new Student(reader.GetString("user_id"), reader.GetString("first_name"), reader.GetString("middle_name"), reader.GetString("last_name"), reader.GetString("street_address"), reader.GetString("brgy_address"), reader.GetString("city_address"), reader.GetString("province_address"), reader.GetString("zip_address"), reader.GetString("country_address"), reader.GetString("department_id_id"));
                    //students.Add(s);
                    //student = s;
                    c = new College(reader.GetString("id"), reader.GetString("name"), reader.GetString("description"));
                    //Debug.Log(c.ID);
                }
            }
            else
            {
                //text.text = "No rows found.";
            }

            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return c;
    }
    public void GetNotification(string id)
    {
        string query = "SELECT * FROM notification where receiver_user_id_id = '" + id + "';";

        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Notification n = new Notification(reader.GetString("id"), reader.GetDateTime("send_datetime"), reader.GetString("isRead"), reader.GetString("sender_user_id_id"), reader.GetString("receiver_user_id_id"), reader.GetString("type_id_id"), reader.GetString("text"));

                    if(n.community!=null)
                    notifications.Add(n);

                }
            }
            else
            {
                //text.text = "No rows found.";
            }

            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public User GetUser(string id)
    {
        string query = "SELECT * FROM user where user_id = '" + id + "';";
        User u = null;

        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    u = new User(reader.GetString("user_id"), null, reader.GetString("user_type"));
                    Debug.Log(u.user_type);
                }
            }
            else
            {
                //text.text = "No rows found.";
            }

            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return u;
    }
    public Community GetCommunity(string id)
    {
        string query = "SELECT * FROM community where user_ptr_id = '" + id + "';";
        Community c = null;

        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    c = new Community(reader.GetString("user_ptr_id"), reader.GetString("name"), reader.GetString("isAdmin"), reader.GetString("isOffice"), reader.GetString("description"), reader.GetString("location"), reader.GetString("isDeleted"));
                }
            }
            else
            {
                //text.text = "No rows found.";
            }
            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return c;
    }
    public void GetSchedule(string id)
    {
        string query = "SELECT * FROM student_schedule where student_id_id = '" + id + "';";
        
        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    //c = new Community(reader.GetString("user_ptr_id"), reader.GetDateTime("date_created"), reader.GetString("name"), reader.GetString("isAdmin"), reader.GetString("isOffice"), reader.GetString("description"), reader.GetString("location"), reader.GetString("isDeleted"));
                    Schedule s = new Schedule(reader.GetString("id"), reader.GetString("event_schedule_id_id"), id);

                    schedules.Add(s);
                }
            }
            else
            {
                //text.text = "No rows found.";
            }
            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public ActivityEvent GetActivityEvent(string id)
    {
        string query = "SELECT * FROM event where activity_ptr_id = '" + id + "' ORDER BY start_date DESC;";
        ActivityEvent a = null;

        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    a = new ActivityEvent(reader.GetString("activity_ptr_id"), reader.GetDateTime("start_date"), reader.GetDateTime("end_date"), reader.GetTimeSpan("start_time"), reader.GetTimeSpan("end_time"), reader.GetString("is_recurring") == "1");
                }
            }
            else
            {
                //text.text = "No rows found.";
            }
            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return a;
    }
    public Activity GetActivity(string id)
    {
        string query = "SELECT * FROM activity where id = '" + id + "';";
        Activity a = null;

        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    DateTime valid = reader.GetDateTime("valid_until_date");
                    string activity_type = reader.GetString("activity_type");
                    string chat_room_id_id = reader.GetString("chat_room_id_id");
                    string parent_activity_id_id = null; //reader.GetString("parent_activity_id_id");

                    string status_id_id = reader.GetString("status_id_id");
                    string creator_community_id = reader.GetString("creator_community_id_id");

                    string status_admin_id_id = null;//reader.GetString("status_admin_id_id");

                    a = new Activity(reader.GetString("id"), reader.GetString("title"), reader.GetString("description"), valid, activity_type, chat_room_id_id, parent_activity_id_id, status_id_id, creator_community_id, status_admin_id_id);
                }
            }
            else
            {
                //text.text = "No rows found.";
            }
            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return a;
    }
    public void GetAnnouncement()
    {
        string query = "SELECT * FROM activity_target_department where department_id = '" + student.department.ID +"';";
        //Activity a = null;
        Debug.Log(student.department.ID);


        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    Activity a = GetActivity(reader.GetString("activity_id"));
                    if(a.activity_type == "Announcement")
                        announcements.Add(a);
                    Debug.Log(a.title);
                }
            }
            else
            {
                //text.text = "No rows found.";
            }
            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void GetActivities()
    {
        string query = "SELECT * FROM activity_student_target where student_id_id = '" + student.user_id + "' order by priority DESC;";
        //Activity a = null;
        Debug.Log(student.department.ID);


        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    Activity a = GetActivity(reader.GetString("activity_id_id"));
                    activities.Add(a);

                    Debug.Log(a.title);
                }
            }
            else
            {
                //text.text = "No rows found.";
            }
            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void GetCommunities()
    {
        string query = "SELECT * FROM community_member where student_id_id = '" + student.user_id + "';";
        //Activity a = null;
        Debug.Log(student.department.ID);


        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    Community a = GetCommunity(reader.GetString("community_id_id"));
                    communities.Add(a);

                    Debug.Log(a.name);
                }
            }
            else
            {
                //text.text = "No rows found.";
            }
            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void GetCommunityList()
    {
        string query = "SELECT * FROM community;";
        //Activity a = null;
        Debug.Log(student.department.ID);


        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {

                while (reader.Read())
                {
                    Community a = GetCommunity(reader.GetString("user_ptr_id"));
                    bool check = true;

                    foreach (Community i in communities)
                    {
                        if (i.user_ptr_id == a.user_ptr_id)
                        {
                            check = false;
                            break;
                        }
                    }

                    if (check)
                        communityList.Add(a);

                    Debug.Log(a.name);
                }
            }
            else
            {
                //text.text = "No rows found.";
            }
            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
    public void GetEvents()
    {
        string query = "SELECT * FROM event;";
        //Activity a = null;


        // Prepare the connection
        MySqlConnection databaseConnection = new MySqlConnection(connectionString);
        MySqlCommand commandDatabase = new MySqlCommand(query, databaseConnection);
        commandDatabase.CommandTimeout = 60;
        MySqlDataReader reader;

        // Let's do it !
        try
        {
            // Open the database
            databaseConnection.Open();

            // Execute the query
            reader = commandDatabase.ExecuteReader();

            // All succesfully executed, now do something

            // IMPORTANT : 
            // If your query returns result, use the following processor :

            if (reader.HasRows)
            {

                while (reader.Read())
                {

                    foreach (Community i in communities)
                    {
                        if (i.user_ptr_id == reader.GetString("creator_community_id"))
                        {
                            if (reader.GetString("status_id_id") == "Approved")
                            {
                                Activity a = GetActivity(reader.GetString("id"));
                            }
                        }
                        else if (reader.GetString("extendToOtherCommunities") == "1")
                        {
                            if (reader.GetString("status_id_id") == "Approved")
                            {
                                Activity a = GetActivity(reader.GetString("id"));
                            }
                        }   
                    }
                }
            }
            else
            {
                //text.text = "No rows found.";
            }
            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}

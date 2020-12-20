using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MySql.Data.MySqlClient;
using System;

public class ViewAllUsers : MonoBehaviour
{
    public GameObject allUsersObject;
    TextMeshProUGUI text ;
    private void Start()
    {
        text = allUsersObject.GetComponent<TextMeshProUGUI>();
    }
    public void ViewUsersDemo()
    {
        text.text = "Blabla";
    }
    public void ViewUsersTrue()
    {
        string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=test;";
        string query = "SELECT * FROM user";

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
                    User user = new User(reader.GetString(0), reader.GetString(1), reader.GetString(2));
                    Debug.Log(reader.GetString("user_id"));
                    text.text += user.ToString();
                    text.text += "\n";
                }
            }
            else
            {
                text.text="No rows found.";
            }

            // Finally close the connection
            databaseConnection.Close();
        }
        catch (Exception e)
        {
            text.text = e.Message;
        }
    }
}

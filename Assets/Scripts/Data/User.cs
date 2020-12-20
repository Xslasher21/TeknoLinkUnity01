using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string id { get; }
    public string password { get; }
    public string user_type { get; }

    public User(string id, string password, string user_type)
    {
        this.id = id;
        this.password = password;
        this.user_type = user_type;
    }
    public override string ToString()
    {
        return "User_id: " + id + ", Password: " + password + ", User_Type: " + user_type;
    }
}

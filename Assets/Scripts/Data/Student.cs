using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student
{
    public string user_id { get; }
    public string first_name { get; }
    public string middle_name { get; }
    public string last_name { get; }
    public string street_address { get; }
    public string brgy_address { get; }
    public string city_address { get; }
    public string province_address { get; }
    public string zip_address { get; }
    public string country_address { get; }

    public Department department { get; }




    public Student(string user_id, string first_name, string middle_name, string last_name, string street_address, string brgy_address, string city_address, string province_address, string zip_address, string country_address, string department_id_id)
    {
        this.user_id = user_id;
        this.first_name = first_name;
        this.middle_name = middle_name;
        this.last_name = last_name;
        this.street_address = street_address;
        this.brgy_address = brgy_address;
        this.city_address = city_address;
        this.province_address = province_address;
        this.zip_address = zip_address;
        this.country_address = country_address;

        department = Database.Instance.GetDepartment(department_id_id);

    }
    public Student()
    {
        first_name = null;
        middle_name = null;
        last_name = null;
        street_address = null;
        brgy_address = null;
        city_address = null;
        province_address = null;
        zip_address = null;
        country_address = null;
        department = null;
    }
}
public class College
{
    public string ID { get; }

    public string Description { get; }
    public string Name { get; }

    public College(string id, string name, string description)
    {
        this.ID = id;
        this.Name = name;
        this.Description = description;
    }
    public College()
    {
        ID = null;
        Name = null;
        Description = null;
    }
}
public class Department
{
    public string ID { get; }

    public string Description { get; }
    public string Name { get; }
    public College college { get; }

    public Department(string department_id, string name, string description, string college_id_id)
    {
        this.ID = department_id;
        this.Name = name;
        this.Description = description;

        college = Database.Instance.GetCollege(college_id_id);
    }
    public Department()
    {
        ID = null;
        Name = null;
        Description = null;
        college = null;
    }

}
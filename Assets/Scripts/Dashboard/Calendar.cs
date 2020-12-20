using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Calendar : MonoBehaviour
{
    [SerializeField] private GameObject schedule = null;
    [SerializeField] private GameObject schedule_parent = null;
    [SerializeField] private TextMeshProUGUI date = null;
    private List<GameObject> scheduleList = new List<GameObject>();

    /// <summary>
    /// Cell or slot in the calendar. All the information each day should now about itself
    /// </summary>
    public class Day
    {
        public int dayNum;
        public Color dayColor;
        public GameObject obj;
        public Button button;
        public List<ActivityEvent> events;
        public TextMeshProUGUI Date;

        /// <summary>
        /// Constructor of Day
        /// </summary>
        public Day(int dayNum, Color dayColor, GameObject obj,TextMeshProUGUI date)
        {
            this.dayNum = dayNum;
            this.obj = obj;
            button = obj.GetComponentInChildren<Button>();
            //Debug.Log(button);
            UpdateColor(dayColor);
            UpdateDay(dayNum);
            events = new List<ActivityEvent>();
            Date = date;
        }

        /// <summary>
        /// Call this when updating the color so that both the dayColor is updated, as well as the visual color on the screen
        /// </summary>
        public void UpdateColor(Color newColor)
        {
            obj.GetComponent<Image>().color = newColor;
            dayColor = newColor;
        }

        /// <summary>
        /// When updating the day we decide whether we should show the dayNum based on the color of the day
        /// This means the color should always be updated before the day is updated
        /// </summary>
        public void UpdateDay(int newDayNum)
        {
            this.dayNum = newDayNum;
            if(dayColor == Color.white || dayColor == Color.green)
            {
                obj.GetComponentInChildren<Text>().text = (dayNum + 1).ToString();
            }
            else
            {
                obj.GetComponentInChildren<Text>().text = "";
            }
        }
    }

    public void AddEvents(List<ActivityEvent> e,Day d)
    {
        foreach (GameObject item in scheduleList)
        {
            Destroy(item);
        }

        scheduleList.Clear();
        d.Date.text = "Day " + (d.dayNum + 1).ToString();

        foreach (ActivityEvent item in e)
        {
            GameObject gb = Instantiate(schedule, schedule_parent.transform);

            ScheduleUI s = gb.GetComponent<ScheduleUI>();
            s.event_name.text = item.activity.title;
            s.time.text = item.start_time.ToString();

            scheduleList.Add(gb);
        }
    }
    /// <summary>
    /// All the days in the month. After we make our first calendar we store these days in this list so we do not have to recreate them every time.
    /// </summary>
    private List<Day> days = new List<Day>();

    /// <summary>
    /// Setup in editor since there will always be six weeks. 
    /// Try to figure out why it must be six weeks even though at most there are only 31 days in a month
    /// </summary>
    public Transform[] weeks;

    /// <summary>
    /// This is the text object that displays the current month and year
    /// </summary>
    public TextMeshProUGUI MonthAndYear;

    /// <summary>
    /// this currDate is the date our Calendar is currently on. The year and month are based on the calendar, 
    /// while the day itself is almost always just 1
    /// If you have some option to select a day in the calendar, you would want the change this objects day value to the last selected day
    /// </summary>
    public DateTime currDate = DateTime.Now;

    /// <summary>
    /// In start we set the Calendar to the current date
    /// </summary>
    private void Start()
    {
        UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);
    }


    /// <summary>
    /// Anytime the Calendar is changed we call this to make sure we have the right days for the right month/year
    /// </summary>
    void UpdateCalendar(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);
        currDate = temp;
        MonthAndYear.text = temp.ToString("MMMM") + " " + temp.Year.ToString();
        int startDay = GetMonthStartDay(year, month);
        int endDay = GetTotalNumberOfDays(year, month);

        date.text = "Date";


        ///Create the days
        ///This only happens for our first Update Calendar when we have no Day objects therefore we must create them

        int x = 0;
        int len = Database.Instance.schedules.Count;
        while (x < len)
        {
            if (temp.ToString("M-y") == Database.Instance.schedules[x].activityEvent.start_date.ToString("M-y"))
            {
                break;
            }
            x++;
        }
        Stack<ActivityEvent> ev = new Stack<ActivityEvent>();
        while (x < len)
        {
            if (temp.ToString("M-y") == Database.Instance.schedules[x].activityEvent.start_date.ToString("M-y"))
            {
                ev.Push(Database.Instance.schedules[x].activityEvent);
                Debug.Log("work: " + len);

            }
            else
            {
                break;
            }
            x++;
        }


        if (days.Count == 0)
        {
            for (int w = 0; w < 6; w++)
            {
                for (int i = 0; i < 7; i++)
                {
                    Day newDay;
                    int currDay = (w * 7) + i;
                    if (currDay < startDay || currDay - startDay >= endDay)
                    {
                        newDay = new Day(currDay - startDay, Color.grey, weeks[w].GetChild(i).gameObject,date);
                    }
                    else
                    {
                        newDay = new Day(currDay - startDay, Color.white, weeks[w].GetChild(i).gameObject,date);
                    }

                    while (ev.Count > 0 && newDay.dayNum + 1 == ev.Peek().GetStartDate().Day)
                    {
                        newDay.events.Add(ev.Pop());
                        newDay.UpdateColor(Color.yellow);

                    }

                    newDay.button.onClick.AddListener(() => AddEvents(newDay.events,newDay));
                    days.Add(newDay);
                }
            }
        }
        ///loop through days
        ///Since we already have the days objects, we can just update them rather than creating new ones
        else
        {
            for (int i = 0; i < 42; i++)
            {
                if (i < startDay || i - startDay >= endDay)
                {
                    days[i].UpdateColor(Color.grey);
                }
                else
                {
                    days[i].UpdateColor(Color.white);
                }

                days[i].UpdateDay(i - startDay);
                days[i].events.Clear();
                while (ev.Count > 0 && days[i].dayNum + 1 == ev.Peek().GetStartDate().Day)
                {
                    days[i].events.Add(ev.Pop());
                    days[i].UpdateColor(Color.yellow);

                }

                //days[i].button.onClick.AddListener(() => AddEvents(days[i].events));

            }
        }

        ///This just checks if today is on our calendar. If so, we highlight it in green
        if (DateTime.Now.Year == year && DateTime.Now.Month == month)
        {
            days[(DateTime.Now.Day - 1) + startDay].UpdateColor(Color.green);
        }

    }
   
    /// <summary>
    /// This returns which day of the week the month is starting on
    /// </summary>
    int GetMonthStartDay(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);

        //DayOfWeek Sunday == 0, Saturday == 6 etc.
        return (int)temp.DayOfWeek;
    }

    /// <summary>
    /// Gets the number of days in the given month.
    /// </summary>
    int GetTotalNumberOfDays(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }

    /// <summary>
    /// This either adds or subtracts one month from our currDate.
    /// The arrows will use this function to switch to past or future months
    /// </summary>
    public void SwitchMonth(int direction)
    {
        if(direction < 0)
        {
            currDate = currDate.AddMonths(-1);
        }
        else
        {
            currDate = currDate.AddMonths(1);
        }

        UpdateCalendar(currDate.Year, currDate.Month);
    }
}

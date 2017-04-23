using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;

    [Header("References")]
    public Light Sun;
    public Text Clock;
    public Text Days;
    public Text LaptopClock;

    [Header("Settings")]
    public float TimeSpeed = 1;


    #region Status
    int Sleep;
    int SelfEsteem;
    int Anxiety;
    #endregion

#region Day Information
    public DateTime DayTime;
    DateTime startDay;
#endregion

    #region Unity Methods
    public void Start()
    {
        DayTime = new DateTime(2017, 4, 22, 8, 0, 0);
        startDay = DayTime;
        singleton = this;
    }

    void Update()
    {
        DayTime = DayTime.AddSeconds(Time.deltaTime * TimeSpeed);
        SetSunPosition();
        SetCanvasInfo();
    }
    #endregion

    #region Private Methods
    void SetSunPosition()
    {
        if (DayTime.Hour >= 8 && DayTime.Hour < 18)
        {
            Sun.transform.rotation = Quaternion.Euler(24, 0, 0);
        }
        else
        {
            Sun.transform.rotation = Quaternion.Euler(230, 0, 0);
        }
    }

    void SetCanvasInfo()
    {
        Clock.text = DayTime.ToString("HH:mm:ss");
        LaptopClock.text = DayTime.ToString("HH:mm:ss \n dd/MM/yyyy");
        Days.text = string.Format("Days: {0}", DayTime.Subtract(startDay).Days);
    }
    #endregion

    #region Actions
    public void Action_DeleteMessage(GameAction action)
    {
        switch (action)
        {
            case GameAction.GoodMessage:
                Debug.Log("Self Esteem decreased");
                break;
            case GameAction.BadMessage:
                Debug.Log("Self Esteem increased");
                break;
            default:
                break;
        }
    }

    public void Action_SkipHours(int hours)
    {
        DayTime = DayTime.AddHours(hours);
    }
    #endregion
}

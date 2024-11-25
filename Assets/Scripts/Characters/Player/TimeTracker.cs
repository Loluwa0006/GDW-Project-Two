using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] TMP_Text textField;
    float currentTime = 0.0f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        TimeSpan timeElasped = TimeSpan.FromSeconds(currentTime);

        string minutesString;
        string secondsString;

        if (timeElasped.Minutes < 10)
        {
            minutesString = "0" + timeElasped.Minutes.ToString();
        }
        else
        {
            minutesString = timeElasped.Minutes.ToString();
        }

        if (timeElasped.Seconds < 10)
        {
            secondsString = "0" + timeElasped.Seconds.ToString();
        }
        else
        {
            secondsString = timeElasped.Seconds.ToString();
        }

        textField.text = minutesString + ":" + secondsString; 
    }
}

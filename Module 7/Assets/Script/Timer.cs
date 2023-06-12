using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    private int hours;
    private int min;
    private int sec;
    public TMP_Text timeDisplay;

    bool timeHasStarted = false;
    float totalTime = 0;
    public void startTimer()
    {
        timeHasStarted = true;
    }
    public void stopTimer()
    {
        timeHasStarted = false;
    }
    public void resetTimer()
    {
        timeHasStarted = false;
        totalTime = 0;
        timeDisplay.SetText("");
    }
    // Update is called once per frame
    void Update()
    {
        if (timeHasStarted)
        {
            totalTime += Time.deltaTime;
            timeDisplay.SetText(((int)totalTime).ToString());
        }
    }
}

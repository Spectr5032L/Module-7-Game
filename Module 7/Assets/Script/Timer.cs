using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
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
    void Update()
    {
        if (timeHasStarted)
        {
            totalTime += Time.deltaTime;
            timeDisplay.SetText(((int)totalTime).ToString());
        }
    }
}

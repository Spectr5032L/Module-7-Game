using UnityEngine;
using TMPro;

public class TimerReverse : MonoBehaviour
{
	public TMP_Text timeDisplay;


	[SerializeField] GameObject losescreen;

	bool timeHasStarted = false;
	float totalTime = 10f;
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
		totalTime = 10f;
		timeDisplay.SetText("");
	}
	public void SetTime(int timeSeconds)
	{
		totalTime = timeSeconds;
		timeDisplay.SetText(((int)totalTime).ToString());
	}

	void Update()
	{
		timeDisplay.color = Color.green;
		if (timeHasStarted)
		{
			totalTime -= Time.deltaTime;
			timeDisplay.SetText(((int)totalTime).ToString());
			if (totalTime < 5)
			{
				timeDisplay.color = Color.red;
			}
			if (totalTime <= 0)
			{
				stopTimer();
				GlobalConstants.gameHasStarted = false;
				losescreen.SetActive(true);
			}
		}
	}
}
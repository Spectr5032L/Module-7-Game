using UnityEngine;

public class Scroll : MonoBehaviour
{
	void Update()
	{
		if (GlobalConstants.gameHasStarted)
		{
			if (Input.mouseScrollDelta.y != 0)
			{
				float scrollAmount = Input.mouseScrollDelta.y;
				gameObject.transform.Translate(0, 0, scrollAmount);
			}
		}
	}
}
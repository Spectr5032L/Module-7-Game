using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeParamsInGameGrid : MonoBehaviour
{

	[SerializeField] Game gameGrid;
	[SerializeField] TMP_InputField widthText;
	[SerializeField] TMP_InputField heightText;
	[SerializeField] TMP_InputField depthText;
	[SerializeField] TMP_InputField mineCount;
	[SerializeField] Slider sensSlider;

	public void updateParams()
	{
		string w = widthText.text;
		string h = heightText.text;
		string d = depthText.text;
		string mc = mineCount.text;

		bool sx, sy, sz, sm;
		sx = int.TryParse(w, out int x);
		sy = int.TryParse(h, out int y);
		sz = int.TryParse(d, out int z);
		sm = int.TryParse(mc, out int m);

		x = sx ? x : -1;
		y = sy ? y : -1;
		z = sz ? z : -1;
		m = sm ? m : -1;

		GlobalConstants.SENSITIVITY = 1000;

		if (sx || sy || sz || sm)
		{
			gameGrid.reinitialize(x < 0 || x > 99 ? 5 : x, y < 0 || y > 99 ? 5 : y, z < 0 || z > 99 ? 5 : z, m < 0 ? 15 : m);
		}
	}
}

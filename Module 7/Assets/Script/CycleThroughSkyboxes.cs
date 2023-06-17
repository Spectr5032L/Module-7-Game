using UnityEngine.UI;
using UnityEngine;

public class CycleThroughSkyboxes : MonoBehaviour
{
    public Material SkyboxHandPaintedBlue;
    public Material SkyboxHandPaintedPurple;
    public Material SkyboxHandPaintedrRed;

	public Button button;
	public Color targetColorBlue;
	public Color targetColorPurple;
	public Color targetColorRed;

	public Canvas Menu;

	private Material[] materials = new Material[3];
    private int pos = 2;

	void ChangeUIColorsBlue()
	{
		// ������� ��� ������� � ����� � ����������� Canvas
		Canvas[] canvases = FindObjectsOfType<Canvas>();

		// ���������� �� ������� ���������� Canvas
		foreach (Canvas canvas in canvases)
		{
			// ������� ��� ������� � ����������� Image ������ �������� Canvas
			Image[] images = canvas.GetComponentsInChildren<Image>();

			// ���������� �� ������� ���������� ������� Image � �������� ��� ����
			foreach (Image image in images)
			{
				image.color = targetColorBlue;
			}
		}
	}

	// ���������� ��� ������� ChangeUIColorsPurple() � ChangeUIColorsRed()

	void ChangeUIColorsPurple()
	{
		// ������� ��� ������� � ����� � ����������� Graphic
		Image[] graphics = FindObjectsOfType<Image>();

		// ���������� �� ������� ���������� ������� � �������� ��� ����
		foreach (Image graphic in graphics)
		{
			graphic.color = targetColorPurple;
		}
	}
	void ChangeUIColorsRed()
	{
		// ������� ��� ������� � ����� � ����������� Graphic
		Image[] graphics = FindObjectsOfType<Image>();

		// ���������� �� ������� ���������� ������� � �������� ��� ����
		foreach (Image graphic in graphics)
		{
			graphic.color = targetColorRed;
		}
	}

	void CheckColor()
	{
		button = GetComponent<Button>();
		if (pos == 3)
		{
			button.onClick.AddListener(ChangeUIColorsBlue);
		}
		if (pos == 1)
		{
			button.onClick.AddListener(ChangeUIColorsPurple);
		}
		if (pos == 2)
		{
			button.onClick.AddListener(ChangeUIColorsRed);
		}
	}

	void Start()
    {
        materials[0] = SkyboxHandPaintedBlue;
		materials[1] = SkyboxHandPaintedPurple;
		materials[2] = SkyboxHandPaintedrRed;

		button = GetComponent<Button>();
		button.onClick.AddListener(ChangeUIColorsRed);

	}
	public void CycleSkybox()
    {
        RenderSettings.skybox = materials[pos++];
		CheckColor();
		pos %= 3;
    }
}

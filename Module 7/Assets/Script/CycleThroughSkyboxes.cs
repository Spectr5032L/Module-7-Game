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
		// Находим все объекты в сцене с компонентом Canvas
		Canvas[] canvases = FindObjectsOfType<Canvas>();

		// Проходимся по каждому найденному Canvas
		foreach (Canvas canvas in canvases)
		{
			// Находим все объекты с компонентом Image внутри текущего Canvas
			Image[] images = canvas.GetComponentsInChildren<Image>();

			// Проходимся по каждому найденному объекту Image и изменяем его цвет
			foreach (Image image in images)
			{
				image.color = targetColorBlue;
			}
		}
	}

	// Аналогично для методов ChangeUIColorsPurple() и ChangeUIColorsRed()

	void ChangeUIColorsPurple()
	{
		// Находим все объекты в сцене с компонентом Graphic
		Image[] graphics = FindObjectsOfType<Image>();

		// Проходимся по каждому найденному объекту и изменяем его цвет
		foreach (Image graphic in graphics)
		{
			graphic.color = targetColorPurple;
		}
	}
	void ChangeUIColorsRed()
	{
		// Находим все объекты в сцене с компонентом Graphic
		Image[] graphics = FindObjectsOfType<Image>();

		// Проходимся по каждому найденному объекту и изменяем его цвет
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

using UnityEngine;

public class InputHandler : MonoBehaviour
{
	Game gameGrid;
	[SerializeField] public Material unknown;
	void Start()
	{
		GlobalConstants.gameHasStarted = false;
		gameGrid = FindObjectOfType<Game>();
	}

	Vector3 mousePosOnClick;
	void Update()
	{
		if (GlobalConstants.gameHasStarted)
		{
			checkForGameInput();
		}
	}

	public void checkForGameInput()
	{
		if (Input.GetButtonDown("LeftClick"))
		{
			mousePosOnClick = Input.mousePosition;
		}
		if (Input.GetButtonUp("LeftClick"))
		{
			if (mousePosOnClick - Input.mousePosition == Vector3.zero)
			{
				Cell c = IsMouseOverGridCell();
				if (c == null)
				{
					return;
				}
				else
				{
					c.leftClicked();
				}
			}
		}

		if (Input.GetButtonUp("RightClick"))
		{
			Cell c = IsMouseOverGridCell();
			if (c == null)
			{
				return;
			}
			else
			{
				c.rightClicked();
			}
		}
	}
	public Cell IsMouseOverGridCell()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 1000f))
		{
			return hit.collider.gameObject.GetComponent<Cell>();
		}
		else
		{
			return null;
		}
	}

}

using UnityEngine;

public class Rotate : MonoBehaviour
{
	private bool mousePressed;
	private float lastMouseX;
	private float lastMouseY;

	void Update()
	{
		if (GlobalConstants.gameHasStarted)
		{
			rotateGrid();
		}
		else
		{
			gameObject.transform.RotateAround(Vector3.zero, Vector3.down, 10 * Time.deltaTime);
			gameObject.transform.RotateAround(Vector3.zero, Vector3.right, 8 * Time.deltaTime);
		}
	}

	private void rotateGrid()
	{
		if (Input.GetButtonDown("LeftClick"))
		{
			lastMouseX = 0;
			lastMouseY = 0;
			mousePressed = true;
		}
		if (Input.GetButtonUp("LeftClick"))
		{
			lastMouseX = Input.GetAxis("Mouse X");
			lastMouseY = Input.GetAxis("Mouse Y");
			mousePressed = false;
		}

		if (mousePressed)
		{
			float mouseX = Input.GetAxis("Mouse X");
			gameObject.transform.RotateAround(Vector3.zero, -Camera.main.transform.up, GlobalConstants.SENSITIVITY * mouseX * Time.deltaTime);
			float mouseY = Input.GetAxis("Mouse Y");
			gameObject.transform.RotateAround(Vector3.zero, Camera.main.transform.right, GlobalConstants.SENSITIVITY * mouseY * Time.deltaTime);
		}
		
	}

}
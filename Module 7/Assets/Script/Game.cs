using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
	public int WIDTH, HEIGHT, DEPTH;
	public int MINE_COUNT;
	private int totalCells;

	//All Possible Materials
	public Material one;
	public Material two;
	public Material three;
	public Material four;
	public Material five;
	public Material six;
	public Material seven;
	public Material eight;
	public Material nine;
	public Material ten;
	public Material eleven;
	public Material twelve;
	public Material thriteen;
	public Material fourteen;
	public Material fifteen;
	public Material sixteen;
	public Material seventeen;
	public Material eighteen;
	public Material nineteen;
	public Material twenty;
	public Material twentyone;
	public Material twentytwo;
	public Material twentythree;
	public Material twentyfour;
	public Material twentyfive;
	public Material twentysix;
	public Material mine;
	public Material unknown;
	public Material transparentUnknown;
	public Material blank;


	[SerializeField] private float CELL_SIZE;
	[SerializeField] Timer timer;
	[SerializeField] GameObject winscreen;
	[SerializeField] GameObject losescreen;

	[SerializeField] private GameObject gridCellPrefab;
	public List<GameObject> revealedCells;

	public GameObject[,,] gameGrid;

	public List<Vector3Int> cellsToCheck = new();
	public List<Vector3Int> cellsToCheckExtended = new();

	private bool[,,] RevealCellIndiciesOnStack;
	void Start()
	{
		//init extended adjacency list
		{
			for (int i = -1; i < 2; i++)
			{
				for (int j = -1; j < 2; j++)
				{
					for (int k = -1; k < 2; k++)
					{
						if (i != 0 || j != 0 || k != 0)
						{
							cellsToCheckExtended.Add(new Vector3Int(i, j, k));
						}
					}
				}
			}
		}
		cellsToCheck.Add(new Vector3Int(1, 0, 0));
		cellsToCheck.Add(new Vector3Int(0, 1, 0));
		cellsToCheck.Add(new Vector3Int(0, 0, 1));
		cellsToCheck.Add(new Vector3Int(-1, 0, 0));
		cellsToCheck.Add(new Vector3Int(0, -1, 0));
		cellsToCheck.Add(new Vector3Int(0, 0, -1));
		createGrid();
		placeMines();
	}


	//Destroys the old game grid and builds a new one
	public void reinitialize(int newWidth, int newHeight, int newDepth, int newMineCount)
	{
		foreach (GameObject g in gameGrid)
		{
			if (g != null)
			{
				Destroy(g);
			}
		}

		WIDTH = newWidth;
		HEIGHT = newHeight;
		DEPTH = newDepth;
		if (newMineCount < WIDTH * HEIGHT * DEPTH)
		{
			MINE_COUNT = newMineCount;
		}

		createGrid();
		placeMines();
	}

	public void resetGame()
	{
		reinitialize(WIDTH, HEIGHT, DEPTH, MINE_COUNT);
		timer.resetTimer();
	}

	private void createGrid()
	{
		revealedCells = new();
		totalCells = WIDTH * DEPTH * HEIGHT;
		// Initialize RevealCellIndiciesOnStack matrix
		{
			RevealCellIndiciesOnStack = new bool[WIDTH, HEIGHT, DEPTH];
			for (int i = 0; i < WIDTH; i++)
			{
				for (int j = 0; j < HEIGHT; j++)
				{
					for (int k = 0; k < DEPTH; k++)
					{
						RevealCellIndiciesOnStack[i, j, k] = false;
					}
				}
			}
		}

		// Initialize gameGrid matrix (stores GameObject -> Cell)
		{
			gameGrid = new GameObject[WIDTH, HEIGHT, DEPTH];
			if (gridCellPrefab == null)
			{
				Debug.LogError("Error: Cell Prefab Not Set");
				return;
			}

			for (int i = 0; i < WIDTH; i++)
			{
				for (int j = 0; j < HEIGHT; j++)
				{
					for (int k = 0; k < DEPTH; k++)
					{
						gameGrid[i, j, k] = Instantiate(gridCellPrefab, new Vector3(i - WIDTH / 2, j - HEIGHT / 2, k - DEPTH / 2) * CELL_SIZE, Quaternion.identity);
						GameObject currentCell = gameGrid[i, j, k];
						currentCell.GetComponent<Cell>().setMatPosition(i, j, k);
						gameGrid[i, j, k].transform.parent = transform;
						gameGrid[i, j, k].gameObject.name = "Cell: { X: " + i + ", Y: " + j + ", Z: " + k + "}";
					}
				}
			}
		}

		// Optimization: only render visible cubes
		// Improves framerate A LOT
		{
			for (int i = 0; i < WIDTH; i++)
			{
				for (int j = 0; j < HEIGHT; j++)
				{
					for (int k = 0; k < DEPTH; k++)
					{
						if (gameGrid[i, j, k] != null)
						{
							gameGrid[i, j, k].SetActive(isVisible(new Vector3Int(i, j, k)));
						}
					}
				}
			}
		}
	}


	//Place n = 0; n < MINE_COUNT; mines randomly
	private void placeMines()
	{
		for (int a = 0; a < MINE_COUNT; a++)
		{
			int x = (int)(Random.value * WIDTH);
			int y = (int)(Random.value * HEIGHT);
			int z = (int)(Random.value * DEPTH);

			if (x == WIDTH || y == HEIGHT || z == DEPTH || gameGrid[x, y, z] == null)
			{ //Deals with possibility of getting 1.0 from Random.value, or selecting a mine that is null(in the mandelbulb case)
				a--;
				continue;
			}

			if (gameGrid[x, y, z] == null)
			{
				a--;
				continue;
			}
			Cell c = gameGrid[x, y, z].GetComponent<Cell>();
			if (c.isMine())
			{
				a--;
				continue;
			}

			c.setAsMine();
			Vector3Int pos = new Vector3Int(x, y, z);

				foreach (Vector3Int adj in cellsToCheck)
				{
					Vector3Int newPos = adj + pos;
					if (isInBounds(newPos) && gameGrid[newPos.x, newPos.y, newPos.z] != null)
					{
						gameGrid[newPos.x, newPos.y, newPos.z].GetComponent<Cell>().incDangerLevel();
					}
				}
		}
	}

	//Takes a position, and checks if all surrounding edges are covered by other cells
	public bool isVisible(Vector3Int cellLocation)
	{
		foreach (Vector3Int adjacency in cellsToCheck)
		{
			Vector3Int pos = adjacency + cellLocation;
			if (isInBounds(pos))
			{
				if (gameGrid[pos.x, pos.y, pos.z] == null)
				{
					return true;

				}
			}
			else
			{
				return true;
			}
		}
		return false;
	}
	//Takes a position, and checks if it is in the bounds of gameGrid
	public bool isInBounds(Vector3Int pos)
	{
		return !(pos.x < 0 || pos.x > WIDTH - 1 || pos.y < 0 || pos.y > HEIGHT - 1 || pos.z < 0 || pos.z > DEPTH - 1);
	}
	//Sets necessary cells visible, and destroys the given cell
	public void destroyCell(Vector3Int pos)
	{
		gameGrid[pos.x, pos.y, pos.z] = null;
		foreach (Vector3Int adjacency in cellsToCheck)
		{
			Vector3Int aPos = adjacency + pos;
			if (isInBounds(aPos))
			{
				if (gameGrid[aPos.x, aPos.y, aPos.z] != null)
				{
					gameGrid[aPos.x, aPos.y, aPos.z].SetActive(true);
				}
			}
		}
	}


	Stack<Vector3Int> operationsToDo = new Stack<Vector3Int>();
	public void revealCell(Vector3Int pos)
	{
		operationsToDo.Push(pos);
		while (operationsToDo.Count > 0)
		{
			Vector3Int newPos = operationsToDo.Pop();
			revCell(newPos);
		}
	}
	private void revCell(Vector3Int pos)
	{
		foreach (Vector3Int adjacency in cellsToCheck)
		{
			Vector3Int aPos = adjacency + pos;
			if (isInBounds(aPos))
			{
				if (gameGrid[aPos.x, aPos.y, aPos.z] != null)
				{
					gameGrid[aPos.x, aPos.y, aPos.z].SetActive(true);
				}
			}
		}
		GameObject c = gameGrid[pos.x, pos.y, pos.z];
		Destroy(c.GetComponent<BoxCollider>());
		int dangerLevel = c.GetComponent<Cell>().getDangerLevel();

		if (dangerLevel == 0)
		{
			destroyCell(pos);
			Destroy(c);
			totalCells--;
			foreach (Vector3Int adj in cellsToCheck)
			{
				Vector3Int newPos = pos + adj;
				if (isInBounds(newPos))
				{
					if (gameGrid[newPos.x, newPos.y, newPos.z] != null)
					{
						if (!RevealCellIndiciesOnStack[newPos.x, newPos.y, newPos.z])
						{
							RevealCellIndiciesOnStack[newPos.x, newPos.y, newPos.z] = true;
							operationsToDo.Push(newPos);
						}
					}
				}
			}
		}
		else
		{
			revealedCells.Add(gameGrid[pos.x, pos.y, pos.z]);
			MeshRenderer m = c.GetComponentInChildren<MeshRenderer>();
			if (m == null)
			{
				Debug.LogError("Mesh Renderer was Null");
				return;
			}
			switch (dangerLevel)
			{
				case -1:
					m.material = mine;
					//TODO
					//GAME ENDS HERE OR SOMETHING
					Debug.Log("not dub");
					stopGame();
					revealAllMines();
					losescreen.SetActive(true);
					return;
				case 1:
					totalCells--;
					m.material = one;
					break;
				case 2:
					totalCells--;
					m.material = two;
					break;
				case 3:
					totalCells--;
					m.material = three;
					break;
				case 4:
					totalCells--;
					m.material = four;
					break;
				case 5:
					totalCells--;
					m.material = five;
					break;
				case 6:
					totalCells--;
					m.material = six;
					break;
				//Cant be above 6 if not in challenge mode (only counting adjacent faces)
				default:
					m.material = blank;
					break;
			}
		}
		if (totalCells == MINE_COUNT && GlobalConstants.gameHasStarted)
		{
			//TODO
			//Game Won
			Debug.Log("dub");
			winscreen.SetActive(true);
			stopGame();
		}
	}
	public void revealAllMines()
	{

		foreach (GameObject obj in gameGrid)
		{
			if (obj != null)
			{
				MeshRenderer m = obj.GetComponentInChildren<MeshRenderer>();
				Cell c = obj.GetComponent<Cell>();
				if (c.getDangerLevel() == 0) m.material = transparentUnknown;
				else if (c.getDangerLevel() == -1) m.material = mine;
				else revealCell(obj.GetComponent<Cell>().getMatPosition());
			}
		}
	}


	public void startGame()
	{
		timer.startTimer();
		GlobalConstants.gameHasStarted = true;
	}
	public void stopGame()
	{
		timer.stopTimer();
		GlobalConstants.gameHasStarted = false;
	}
}
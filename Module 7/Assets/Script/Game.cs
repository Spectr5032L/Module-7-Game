using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public int WIDTH, HEIGHT, DEPTH;
	public int MINE_COUNT;
	private int totalCells;

	public Material one;
	public Material two;
	public Material three;
	public Material four;
	public Material five;
	public Material six;
	public Material mine;
	public Material unknown;
	public Material transparentUnknown;


	[SerializeField] private float CELL_SIZE;
	[SerializeField] Timer timer;
	[SerializeField] TimerReverse TimerReverse;
	[SerializeField] GameObject winscreen;
	[SerializeField] GameObject losescreen;

	[SerializeField] private GameObject gridCellPrefab;

	public GameObject[,,] gameGrid;

	public List<Vector3Int> cellsToCheck = new();

	private bool[,,] RevealCellIndiciesOnStack;
	void Start()
	{
		cellsToCheck.Add(new Vector3Int(1, 0, 0));
		cellsToCheck.Add(new Vector3Int(0, 1, 0));
		cellsToCheck.Add(new Vector3Int(0, 0, 1));
		cellsToCheck.Add(new Vector3Int(-1, 0, 0));
		cellsToCheck.Add(new Vector3Int(0, -1, 0));
		cellsToCheck.Add(new Vector3Int(0, 0, -1));
		createGrid();
		placeMines();
	}


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
		TimerReverse.resetTimer();
	}

	private void createGrid()
	{
		totalCells = WIDTH * DEPTH * HEIGHT; //вычисление количества клеток
		RevealCellIndiciesOnStack = new bool[WIDTH, HEIGHT, DEPTH]; //массив открытых/неоткрытых ел++клеток булев
		for (int i = 0; i < WIDTH; i++)
		{
			for (int j = 0; j < HEIGHT; j++)
			{
				for (int k = 0; k < DEPTH; k++)
				{
					RevealCellIndiciesOnStack[i, j, k] = false; // задается значение фолс для всех клеток куба
				}
			}
		}

		gameGrid = new GameObject[WIDTH, HEIGHT, DEPTH]; // задаются координаты в пространстве для клеток

		for (int i = 0; i < WIDTH; i++)
		{
			for (int j = 0; j < HEIGHT; j++)
			{
				for (int k = 0; k < DEPTH; k++)
				{
					gameGrid[i, j, k] = Instantiate(gridCellPrefab, new Vector3(i - WIDTH / 2, j - HEIGHT / 2, k - DEPTH / 2) * CELL_SIZE, Quaternion.identity); // для геймгрида приклеивается префаб и умножается на размер
					GameObject currentCell = gameGrid[i, j, k]; // клетка - игровой объект
					currentCell.GetComponent<Cell>().setMatPosition(i, j, k);
					gameGrid[i, j, k].transform.parent = transform;
				}
			}
		}
	}


	private void placeMines() // расставлять мины
	{
		for (int a = 0; a < MINE_COUNT; a++) // перебираем количество мин и выбираем рандомные координаты
		{
			int x = (int)(Random.value * WIDTH);
			int y = (int)(Random.value * HEIGHT);
			int z = (int)(Random.value * DEPTH);

			if (x == WIDTH || y == HEIGHT || z == DEPTH || gameGrid[x, y, z] == null) // если трем полученным координатам соответствует куб, то все хорошо
			{ 
				a--;
				continue;
			}

			Cell c = gameGrid[x, y, z].GetComponent<Cell>(); // присваивается материал мины
			if (c.isMine())
			{
				a--;
				continue;
			}

			c.setAsMine();
			Vector3Int pos = new Vector3Int(x, y, z); // вектор позиции мины

				foreach (Vector3Int adj in cellsToCheck) // перебор по координатам
				{
					Vector3Int newPos = adj + pos; // находит соседнюю клетку
					if (isInBounds(newPos))
					{
						gameGrid[newPos.x, newPos.y, newPos.z].GetComponent<Cell>().incDangerLevel(); // повышается уровень опасности клетки
					}
				}
		}
	}

	public bool isInBounds(Vector3Int pos) // не выходит за грань кубика
	{
		return !(pos.x < 0 || pos.x > WIDTH - 1 || pos.y < 0 || pos.y > HEIGHT - 1 || pos.z < 0 || pos.z > DEPTH - 1);
	}
	public void destroyCell(Vector3Int pos) // уничтожение клетки
	{
		gameGrid[pos.x, pos.y, pos.z] = null;
		foreach (Vector3Int adjacency in cellsToCheck)
		{
			Vector3Int aPos = adjacency + pos;
			if (isInBounds(aPos)) // проверка соседних клеток на безопасность
			{
				if (gameGrid[aPos.x, aPos.y, aPos.z] != null) // если у клетки уровень опасности 0, клетка открывается
				{
					gameGrid[aPos.x, aPos.y, aPos.z].SetActive(true); // клетка автоматически открывается
				}
			}
		}
	}

	Stack<Vector3Int> operationsToDo = new Stack<Vector3Int>(); // стек клеток, которые нужно открыть
	public void revealCell(Vector3Int pos) // открывает клетку в зависимости от значения клетки, в cell вызывается
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
			MeshRenderer m = c.GetComponentInChildren<MeshRenderer>();
			if (m == null)
			{
				return;
			}
			switch (dangerLevel)
			{
				case -1:
					m.material = mine;
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
				default:
					m.material = transparentUnknown;
					break;
			}
		}
		if (totalCells == MINE_COUNT && GlobalConstants.gameHasStarted)
		{
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
		TimerReverse.startTimer();
	}
	public void stopGame()
	{
		timer.stopTimer();
		GlobalConstants.gameHasStarted = false;
		TimerReverse.stopTimer();
	}
}
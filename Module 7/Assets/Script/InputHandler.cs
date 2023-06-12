using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    Game gameGrid;
    [SerializeField] private LayerMask gridLayer;
    [SerializeField] private LayerMask menuLayer;
    [SerializeField] public Material unknown;
    private bool shiftPressed = false;
    private List<GameObject> shiftVisible = new();
    private Cell oldCell;
    // Start is called before the first frame update
    void Start()
    {
        GlobalConstants.gameHasStarted = false;
        gameGrid = FindObjectOfType<Game>();
    }

    // Update is called once per frame
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
                    Debug.Log("Game: Clicked Empty Space");
                    return;
                }
                else
                {
                    Debug.Log("Clicked: " + c.name + " dlvl: " + c.getDangerLevel());
                    c.leftClicked();
                }
            }
            else
            {
                Debug.Log("Moved Camera");
            }
        }

        if (Input.GetButtonUp("RightClick"))
        {
            Cell c = IsMouseOverGridCell();
            if (c == null)
            {
                Debug.Log("Flagged Empty Space");
                return;
            }
            else
            {
                Debug.Log("Flagged: " + c.name);
                c.rightClicked();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            shiftVisible.Clear();
            oldCell = null;
            shiftPressed = true;
            foreach(GameObject obj in gameGrid.revealedCells)
            {
                obj.SetActive(false);
            }
        }
        if (shiftPressed)
        {
            Cell c = IsMouseOverGridCell();
            if (c == null)
            {
                foreach (GameObject obj in shiftVisible)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                if (!c.Equals(oldCell))
                {
                    oldCell = c;
                    foreach (GameObject obj in shiftVisible)
                    {
                        obj.SetActive(false);
                    }
                    shiftVisible.Clear();
                    Vector3Int pos = c.getMatPosition();
                    foreach (Vector3Int adj in gameGrid.cellsToCheckExtended)
                    {
                        Vector3Int newPos = pos + adj;
                        if (gameGrid.isInBounds(newPos))
                        {
                            if (gameGrid.gameGrid[newPos.x, newPos.y, newPos.z] != null)
                            {
                                if (gameGrid.revealedCells.Contains(gameGrid.gameGrid[newPos.x, newPos.y, newPos.z]))
                                {
                                    gameGrid.gameGrid[newPos.x, newPos.y, newPos.z].SetActive(true);
                                    shiftVisible.Add(gameGrid.gameGrid[newPos.x, newPos.y, newPos.z]);
                                }
                            }
                        }
                    }
                }
               
            }

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            shiftPressed = false;
            foreach (GameObject obj in gameGrid.revealedCells)
            {
                obj.SetActive(true);
            }
            
        }
    }
    public Cell IsMouseOverGridCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit, 1000f, gridLayer)){
            return hit.collider.gameObject.GetComponent<Cell>();
        }
        else
        {
            return null;
        }
    }

}

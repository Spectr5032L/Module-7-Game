using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private Vector3Int matPosition;
    private int dangerLevel = 0;
    public Material flag;
    public Material unknown;
    bool isFlagged = false;

    public void leftClicked()
    {
        gameObject.GetComponentInParent<Game>().revealCell(matPosition);
    }
    public void rightClicked()
    {
        MeshRenderer m = gameObject.GetComponentInChildren<MeshRenderer>();
        if (isFlagged)
        {
            isFlagged = false;
            m.material = unknown;
        }
        else
        {
            isFlagged = true;
            m.material = flag;
        }
    }
    public void setMatPosition(int x, int y, int z)
    {
        matPosition = new Vector3Int(x, y, z);
    }
    public Vector3Int getMatPosition()
    {
        return matPosition;
    }
    public void incDangerLevel()
    {
        if (!isMine()) dangerLevel++;
    }
    public int getDangerLevel()
    {
        return dangerLevel;
    }

    public bool isMine()
    {
        return dangerLevel == -1;
    }
    public void setAsMine()
    {
        dangerLevel = -1;
    }
}

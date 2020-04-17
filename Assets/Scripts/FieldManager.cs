using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
internal struct FieldCell
{
    internal bool isFull;
    internal GameObject figure;
}

public class FieldManager : MonoBehaviour
{
    [SerializeField] Pattern[] patterns;
    [SerializeField] float timerDelay = 0.7f;
    [SerializeField] int width = 10;
    [SerializeField] int height = 15;
    [SerializeField] UI ui;
    Pattern currentFigure;
    Vector2 currentFigurePosition;

    FieldCell[,] field;
    List<Transform> figures;
    void Start()
    {
        figures = new List<Transform>();
        field = new FieldCell[width, height];
        createNewFigure();
        StartCoroutine(timer());
    }

    private void Update()
    {
        figureMove();
    }

    private void createNewFigure()
    {
        int rand = Random.Range(0, patterns.Length);
        currentFigure = Instantiate(patterns[rand]);
        currentFigure.generateFigure();
        currentFigurePosition = new Vector2((int) width / 2, height - 1);
        moveFigure(0, 0);
    }

    IEnumerator timer()
    {
        yield return new WaitForSeconds(timerDelay);
        calculateField();
        StartCoroutine(timer());
    }

    private void calculateField()
    {
        if (figureCheck(currentFigure.getPatternArray(), new Vector2(0, -1)))
        {
            moveFigure(0, -1);
        } else
        {
            if (currentFigurePosition.y == height - 1)
            {
                restart();
            }
            else
            {
                saveFigure();
                checkRows();
                createNewFigure();
            }
        }
    }
    
    private void checkRows()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            checkRow(i);
        }
    }

    private bool checkRow(int row)
    {
        for (int i = 0; i < width; i++)
        {
            if (!field[i, row].isFull) return false;
        }

        destroyRow(row);
        return true;
    }

    private void destroyRow(int row)
    {
        ui.addPoints();
        for (int i = 0; i < width; i++)
        {
            field[i, row].isFull = false;
            Destroy(field[i, row].figure);           
        }
        shiftRow(row);
    }

    private void shiftRow(int row)
    {
        for (int i = row; i < height - 1; i++)
        {
            for (int j = 0; j < width; j++)
            {
                field[j, i].isFull = field[j, i + 1].isFull;
                field[j, i].figure = field[j, i + 1].figure;

                if (field[j, i].figure != null)
                    field[j, i].figure.transform.Translate(0, -1, 0);
            }
        }
        for (int j = 0; j < width; j++)
        {
            Destroy(field[j, height - 1].figure);
            field[j, height - 1].isFull = false;
        }
    }

    private void figureMove()
    {
        if (Input.GetKeyDown(KeyCode.J)
            && figureCheck(currentFigure.getPatternArray(), new Vector2(-1, 0)))
        {
            moveFigure(-1, 0);
        }

        if (Input.GetKeyDown(KeyCode.L)
            && figureCheck(currentFigure.getPatternArray(), new Vector2(1, 0)))
        {
            moveFigure(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.I)
            && figureCheck(rotateFigure(currentFigure.getPatternArray()), new Vector2(0, 0))) {
            currentFigure.changePattern(rotateFigure(currentFigure.getPatternArray()));
        }
    }

    private void saveFigure()
    {
        bool[,] currentFigurePattern = currentFigure.getPatternArray();
        for (int i = 0; i < currentFigurePattern.GetLength(0); i++)
        {
            for (int j = 0; j < currentFigurePattern.GetLength(1); j++)
            {
                if (currentFigurePattern[i, j])
                {
                    field[i + (int)currentFigurePosition.x, (int)currentFigurePosition.y - j].isFull = true;
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = 
                        new Vector3(currentFigure.transform.position.x + i, currentFigure.transform.position.y - j, 0);
                    cube.GetComponent<MeshRenderer>().material.color = currentFigure.color;
                    field[i + (int)currentFigurePosition.x, (int)currentFigurePosition.y - j].figure = cube;
                }
            }
        }
        Destroy(currentFigure.gameObject);
    }

    private void moveFigure(int x, int y)
    {
        currentFigurePosition += new Vector2(x, y);
        currentFigure.transform.position = 
            new Vector3(0.5f + currentFigurePosition.x - width / 2, 0.5f + currentFigurePosition.y, 0);
    }

    private bool figureCheck(bool[,] currentFigurePattern, Vector2 displacement)
    {
        Vector2 position = currentFigurePosition + displacement;
        for (int i = 0; i < currentFigurePattern.GetLength(0); i++)
        {
            for (int j = 0; j < currentFigurePattern.GetLength(1); j++)
            {
                try
                {
                    if (field[i + (int)position.x, (int)position.y - j].isFull
                    && currentFigurePattern[i, j])
                    {
                        return false;
                    }
                } catch (System.Exception ex)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool[,] rotateFigure(bool[,] figure)
    {
        bool[,] figureRotate = new bool[figure.GetLength(1), figure.GetLength(0)];
        for (int i = 0; i < figure.GetLength(0); i++)
        {
            for (int j = figure.GetLength(1) - 1; j >= 0; j--)
            {
                figureRotate[j, i] = figure[i, figure.GetLength(1) - 1 - j];
            }
        }
        return figureRotate;
    }

    private void restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}
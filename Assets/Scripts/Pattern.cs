using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour
{
    [SerializeField] internal Color color;
    [SerializeField] string[] columns;
    List<GameObject> cubes;

    bool[,] patternArray;

    internal bool[,] getPatternArray()
    {
        return patternArray;
    }

    private bool[,] generatePatternArray()
    {
        int width = columns[0].Length;
        int height = columns.Length;
        bool[,] _patternArray = new bool[width, height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (columns[i][j].Equals('1'))
                {
                    _patternArray[j, i] = true;
                }
                else
                {
                    _patternArray[j, i] = false;
                }
            }
        }
        return _patternArray;
    }
    void paintCubes(Color color)
    {
        foreach (GameObject cub in cubes)
        {
            cub.GetComponent<MeshRenderer>().material.color = color;
        }
    }

    private void setColor(Color color)
    {
        this.color = color;
        paintCubes(color);
    }

    internal void generateFigure()
    {
        color = new Color(Random.Range(0, 2), Random.Range(0, 2), Random.Range(0, 2));
        cubes = new List<GameObject>();
        patternArray = generatePatternArray();
        renderCubes();
        setColor(color);
    }

    internal void changePattern(bool[,] patternArray)
    {
        destroyChilds();
        cubes = new List<GameObject>();
        this.patternArray = patternArray;
        renderCubes();
        setColor(color);
    }

    internal void cutRow(int rowNumber)
    {
        bool[,] cutPatternArray = new bool[patternArray.GetLength(0), patternArray.GetLength(1) - 1];
    }

    private void destroyChilds()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void renderCubes()
    {
        for (int i = 0; i < patternArray.GetLength(1); i++)
        {
            for (int j = 0; j < patternArray.GetLength(0); j++)
            {
                if (patternArray[j, i])
                {
                    GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.parent = this.transform;
                    obj.transform.localPosition = new Vector3(j, -i, 0);
                    cubes.Add(obj);
                }
            }
        }
    }
}

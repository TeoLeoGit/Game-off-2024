using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GridGenerator : MonoBehaviour
{
    public PathFinder pathFinder;
    public Cell cellPrefab; // Prefab for each grid cell
    public List<PathNode> pathNodes = new();

    public Dictionary<Vector2, PathNode> _pathNodeMap = new();
    public Dictionary<Vector2, Cell> _visualizeCells = new();
    public int columns = 9;
    public int rows = 9;
    private void Awake()
    {
        foreach (var node in pathNodes)
        {
            _pathNodeMap.Add(new Vector2(node.x, node.y), node);
        }
    }

    public void GenerateGrid()
    {
        // Clear existing children
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
        _pathNodeMap.Clear();
        _visualizeCells.Clear();
        pathNodes.Clear();

        // Generate grid
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                Vector2 position = new Vector3(x, y);
                var instant = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                instant.SetCell(x, y, this);
                var pathNode = new PathNode(x, y);
                _pathNodeMap.Add(position, pathNode);
                _visualizeCells.Add(position, instant);
                pathNodes.Add(pathNode);
            }
        }
    }

    public void InitPathNodeCost()
    {
        foreach (var node in _pathNodeMap)
        {
            node.Value.gCost = int.MaxValue;
            node.Value.CalculateFCost();
            node.Value.prevNode = null;
        }
    }

    public void TestPath()
    {

        //Generate wall
        for (int i = 0; i < 20; i++)
        {
            var pos = new Vector2(UnityEngine.Random.Range(0, columns), UnityEngine.Random.Range(0, rows));
            _pathNodeMap[pos].isWall = true;
            _visualizeCells[pos].IsWall = true;
        }

        var result = pathFinder.FindPath(new Vector2(0, 1), new Vector2(8, 6));
        if (result != null)
        foreach (var item in result)
        {
            _visualizeCells[new Vector2(item.x, item.y)].IsPath = true;
        }
    }

    public PathNode GetNodeAtGrid(int x, int y)
    {
        return _pathNodeMap[new Vector2(x, y)];
    }
}


#if UNITY_EDITOR

[CustomEditor(typeof(GridGenerator))]
public class GridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridGenerator gridGenerator = (GridGenerator)target;
        if (GUILayout.Button("ADD"))
        {
            gridGenerator.GenerateGrid();
        }
        if (GUILayout.Button("CLEAR"))
        {
            foreach (Transform child in gridGenerator.transform)
            {
                DestroyImmediate(child.gameObject);
            }
            gridGenerator._pathNodeMap.Clear();
            gridGenerator._visualizeCells.Clear();
            gridGenerator.pathNodes.Clear();
        }
        if (GUILayout.Button("FIND PATH"))
        {
            gridGenerator.TestPath();
        }
        if (GUILayout.Button("REMOVE VISUALIZE"))
        {
            foreach (Transform child in gridGenerator.transform)
            {
                DestroyImmediate(child.gameObject);
                gridGenerator._visualizeCells.Clear();
            }
        }
    }
}
#endif


[Serializable]
public class PathNode
{
    public int x;
    public int y;
    public bool isWall;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode prevNode;

    public PathNode(int x, int y, bool isWall = false)
    {
        this.x = x;
        this.y = y;
        this.isWall = isWall;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
}
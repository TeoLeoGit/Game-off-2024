using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private const int MOVE_STRAIGHT_WEIGHT = 10;
    private const int MOVE_DIAGONAL_WEIGHT = 14;
    [SerializeField] GridGenerator _gridGenerator;

    private int CalculateDistance(PathNode node1, PathNode node2)
    {
        int x = Mathf.Abs(node1.x - node2.y);
        int y = Mathf.Abs(node1.y - node2.y);
        int remaining = Mathf.Abs(x - y);

        return MOVE_DIAGONAL_WEIGHT * Mathf.Min(x, y) + MOVE_STRAIGHT_WEIGHT * remaining;
    }

    public List<PathNode> FindPath(Vector2 startPos, Vector2 endPos)
    {
        PathNode startNode = _gridGenerator._pathNodeMap[startPos];
        PathNode endNode = _gridGenerator._pathNodeMap[endPos];

        var nodesToSearch = new List<PathNode> { startNode };
        var searchedNodes = new List<PathNode>();

        _gridGenerator.InitPathNodeCost();

        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startNode, endNode);
        startNode.CalculateFCost();

        while (nodesToSearch.Count > 0)
        {
            PathNode current = GetLowestFCostNode(nodesToSearch);
            if (current == endNode)
            {
                return CalCulatePath(endNode);
            }

            nodesToSearch.Remove(current);
            searchedNodes.Add(current);

            foreach (var neighborNode in SearchNodeNeighbors(current))
            {
                if (searchedNodes.Contains(neighborNode)) continue;
                if (neighborNode.isWall)
                {
                    searchedNodes.Add(neighborNode);
                    continue;
                }
                int tentitiveGCost = current.gCost + CalculateDistance(current, neighborNode);
                if (tentitiveGCost < neighborNode.gCost) 
                {
                    neighborNode.prevNode = current;
                    neighborNode.gCost = tentitiveGCost;
                    neighborNode.hCost = CalculateDistance(neighborNode, endNode);
                    neighborNode.CalculateFCost();

                    if (!nodesToSearch.Contains(neighborNode))
                    {
                        nodesToSearch.Add(neighborNode);
                    }
                }
            }
        }

        return null;
    }

    private List<PathNode> SearchNodeNeighbors(PathNode nodeToSearch)
    {
        List<PathNode> neighborNodes = new List<PathNode>();
        if (nodeToSearch.x >= 1)
        {
            neighborNodes.Add(_gridGenerator.GetNodeAtGrid(nodeToSearch.x - 1, nodeToSearch.y));
            if (nodeToSearch.y >= 1)
                neighborNodes.Add(_gridGenerator.GetNodeAtGrid(nodeToSearch.x - 1, nodeToSearch.y - 1));
            if (nodeToSearch.y < _gridGenerator.columns - 1)
                neighborNodes.Add(_gridGenerator.GetNodeAtGrid(nodeToSearch.x - 1, nodeToSearch.y + 1));
        }
        if (nodeToSearch.x < _gridGenerator.rows - 1)
        {
            neighborNodes.Add(_gridGenerator.GetNodeAtGrid(nodeToSearch.x + 1, nodeToSearch.y));
            if (nodeToSearch.y >= 1)
                neighborNodes.Add(_gridGenerator.GetNodeAtGrid(nodeToSearch.x + 1, nodeToSearch.y - 1));
            if (nodeToSearch.y < _gridGenerator.columns - 1)
                neighborNodes.Add(_gridGenerator.GetNodeAtGrid(nodeToSearch.x + 1, nodeToSearch.y + 1));
        }
        if (nodeToSearch.y >= 1)
            neighborNodes.Add(_gridGenerator.GetNodeAtGrid(nodeToSearch.x, nodeToSearch.y - 1));
        if (nodeToSearch.y <= _gridGenerator.columns - 1)
            neighborNodes.Add(_gridGenerator.GetNodeAtGrid(nodeToSearch.x, nodeToSearch.y + 1));
        return neighborNodes;
    }

    private List<PathNode> CalCulatePath(PathNode endNode)
    {
        List<PathNode> pathNodes = new List<PathNode>() { endNode };
        var current = endNode; 
        while (current.prevNode != null) 
        { 
            pathNodes.Add(current.prevNode);
            current = current.prevNode;
        }
        pathNodes.Reverse();
        return pathNodes;
    }

    public PathNode GetLowestFCostNode(List<PathNode> pathNodes)
    {
        var result = pathNodes[0];
        foreach (var item in pathNodes)
        {
            if (item.fCost < result.fCost)
                result = item;
        }
        return result;
    }
}

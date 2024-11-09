using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    public static event Action<Vector2> OnPlayerPositionUpdate;
    public static event Action<Vector2> OnPathNodeReach;

    private static PathFinder _pathFinder;
   

    public static List<PathNode> RequestPathToPosition(Vector2 startPos, Vector2 endPos)
    {
        return _pathFinder.FindPath(startPos, endPos);
    }

    public static void SetGamePathFinder(PathFinder pathFinder)
    {
        _pathFinder = pathFinder;
    }

    public static void CallOnPlayerPositonUpdate(Vector2 pos)
    {
        OnPlayerPositionUpdate?.Invoke(pos);
    }

    public static void CallOnReachPathNode(Vector2 gridPos)
    {
        OnPathNodeReach?.Invoke(gridPos);
    }
}

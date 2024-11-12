using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    public static event Action<Vector2> OnPlayerPositionUpdate;
    public static event Action<Vector2> OnPathNodeReach;
    public static event Action<int, int> OnPlayerEnterMapTrigger;
    public static event Action<int> OnChangeMap;
    
    //Shared references
    private static PathFinder _pathFinder;
    private static GameLevel _gameLevel;
   
    public static List<PathNode> RequestPathToPosition(Vector2 startPos, Vector2 endPos)
    {
        return _pathFinder.FindPath(startPos, endPos);
    }

    public static Vector2 GetEntranceWorldPosition(int mapId, int entranceId)
    {
        return _gameLevel.GetWorldPosition(mapId, entranceId);
    }

    public static Vector2 GetEntranceGridPosition(int mapId, int entranceId)
    {
        return _gameLevel.GetGridPosition(mapId, entranceId);
    }

    public static void SetGamePathFinder(PathFinder pathFinder)
    {
        _pathFinder = pathFinder;
    }

    public static void SetGameLevel(GameLevel gameLevel)
    {
        _gameLevel = gameLevel;
    }

    public static void CallOnPlayerPositonUpdate(Vector2 pos)
    {
        OnPlayerPositionUpdate?.Invoke(pos);
    }

    public static void CallOnReachPathNode(Vector2 gridPos)
    {
        OnPathNodeReach?.Invoke(gridPos);
    }

    public static void CallChangeMap(int mapId)
    {
        OnChangeMap?.Invoke(mapId);
    }

    public static void CallPlayerEnterMapTrigger(int mapId, int entranceId)
    {
        OnPlayerEnterMapTrigger?.Invoke(mapId, entranceId);
    }
}

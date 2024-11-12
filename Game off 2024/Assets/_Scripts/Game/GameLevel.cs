using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour
{
    [SerializeField] int _mapId;
    [SerializeField] List<ChangeMapTrigger> _entrances;

    public Vector3 GetWorldPosition(int mapId, int entranceId)
    {
        var match = _entrances.Find(item => item.MapId == mapId && item.EntranceId == entranceId);
        return match.WorldSpawnPos;
    }

    public Vector2 GetGridPosition(int mapId, int entranceId)
    {
        var match = _entrances.Find(item => item.MapId == mapId && item.EntranceId == entranceId);
        return match.GridPos;
    }

}

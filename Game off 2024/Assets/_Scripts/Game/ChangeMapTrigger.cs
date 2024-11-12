using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMapTrigger : MonoBehaviour
{
    [SerializeField] private int _entranceId;
    [SerializeField] private int _mapId;
    [SerializeField] private int _toMapId;
    [SerializeField] Vector2 _gridPos;
    [SerializeField] Vector2 _worldSpawnPos;

    public int EntranceId { get { return _entranceId; } } 
    public int MapId { get { return _mapId; } } 
    public Vector2 GridPos { get { return _gridPos; } }
    public Vector2 WorldSpawnPos { get { return _worldSpawnPos; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Change map
        Debug.Log("OK!");
        Debug.Log($"Check {_toMapId}, {_entranceId}");
        GameController.CallStartMap(_toMapId, _entranceId);
    }
}

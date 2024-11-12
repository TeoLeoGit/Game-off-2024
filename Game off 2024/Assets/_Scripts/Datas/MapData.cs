using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "ScriptableObjects/MapData")]
public class MapData : ScriptableObject
{
    [SerializeField] private List<MapInfo> _gameMaps;
    public List<MapInfo> Maps { get { return _gameMaps; } }

}

[Serializable]
public class MapInfo
{
    public int mapId;
    public GameObject mapPrefab;
}

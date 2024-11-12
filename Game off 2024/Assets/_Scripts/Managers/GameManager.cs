using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] MapData _mapData;
    [SerializeField] Transform _gameContainer;

    [Space(20)]

    [Header("Instantiate references")]
    [SerializeField] GameObject _playerPrefab;

    private Dictionary<int, MapInfo> _mapInfos = new();
    private Dictionary<int, GameObject> _instantiatedMap = new();

    //Caching
    private GameObject _currentActiveMap;
    private GameObject _player;

    private void Awake()
    {
        GameController.OnStartMap += OnStartMap;
    }

    private void OnDestroy()
    {
        GameController.OnStartMap -= OnStartMap;
    }

    private void Start()
    {
        foreach(var map in _mapData.Maps)
        {
            _mapInfos.Add(map.mapId, map);
        }
    }

    private void InstantiateMap(int mapId)
    {
        if (_instantiatedMap.ContainsKey(mapId)) return;
        GameObject map = Instantiate(_mapInfos[mapId].mapPrefab, _gameContainer);
        _instantiatedMap.Add(mapId, map);
    }

    private void OnStartMap(int mapId, int entranceId)
    {
        _currentActiveMap?.SetActive(false);
        InstantiateMap(mapId);
        _instantiatedMap[mapId].SetActive(true);
        _currentActiveMap = _instantiatedMap[mapId];
        GameController.SetGameLevel(_currentActiveMap.GetComponent<GameLevel>());
        MovePlayerToMap(mapId, entranceId);
    }

    private void MovePlayerToMap(int mapId, int entranceId)
    {
        if (_player == null)
        {
            _player = Instantiate(_playerPrefab, _gameContainer);
        }

        var gridPos = GameController.GetEntranceGridPosition(mapId, entranceId);
        var worldPos = GameController.GetEntranceWorldPosition(mapId, entranceId);
        _player.GetComponent<CharacterMovement>().SetPosition(worldPos, gridPos);
    }
}

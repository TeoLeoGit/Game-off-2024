using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _prefabLevel;
    private GameObject _currentLevel;

    private void Awake()
    {
        MainController.OnNewGame += InstantiateLevel;
        MainController.OnReturnHome += DestroyLevel;
    }

    private void OnDestroy()
    {
        MainController.OnNewGame -= InstantiateLevel;
        MainController.OnReturnHome -= DestroyLevel;
    }

    void InstantiateLevel()
    {
        _currentLevel = Instantiate(_prefabLevel, transform);
        _currentLevel.transform.position = new Vector3(-5, 2, 0);
    }

    void DestroyLevel()
    {
        Destroy(_currentLevel);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeView : MonoBehaviour
{
    [SerializeField] private Button _btnPlay;
    [SerializeField] private Button _btnQuit;

    private void Awake()
    {
        _btnPlay.onClick.AddListener(OnStartNewGame);

        MainController.OnShowHome += OnReturnHome;
    }

    private void OnDestroy()
    {
        MainController.OnShowHome -= OnReturnHome;
    }

    private void OnStartNewGame()
    {
        gameObject.SetActive(false);
        MainController.StartGame();
    }

    private void OnReturnHome()
    {
        gameObject.SetActive(true);
    }
}

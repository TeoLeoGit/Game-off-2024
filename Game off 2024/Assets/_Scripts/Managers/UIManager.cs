using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _homeUI;
    [SerializeField] GameObject _gameUI;
    [SerializeField] GameObject _popupEnd;


    [SerializeField] Button _btnPlay;
    [SerializeField] Button _btnQuit;
    [SerializeField] Button _btnHome;


    [SerializeField] Text _txtScoreValue;
    [SerializeField] Text _txtScoreValuePopupEnd;


    [SerializeField] Image _imgColorToRun;

    [SerializeField] List<GameObject> _lifes;

    private int _currentScore = 0;
    private int _currentLife = 4;


    private void Awake()
    {
        MainController.OnNewGame += OnStartNewGame;
        MainController.OnReturnHome += OnReturnHome;
        MainController.OnUpdateLifeView += UpdateLife;
        MainController.OnEndGame += ShowPopupEndGame;

        GameController.OnScoreUpdate += UpdateScore;
        GameController.OnChangePlayerColor += UpdateColor;
    }

    private void OnDestroy()
    {
        MainController.OnNewGame -= OnStartNewGame;
        MainController.OnReturnHome -= OnReturnHome;
        MainController.OnUpdateLifeView -= UpdateLife;
        MainController.OnEndGame -= ShowPopupEndGame;

        GameController.OnScoreUpdate -= UpdateScore;
        GameController.OnChangePlayerColor -= UpdateColor;
    }

    private void Start()
    {
        _btnPlay.onClick.AddListener(() => MainController.StartNewGame());
        _btnQuit.onClick.AddListener(() => CloseApplication());
        _btnHome.onClick.AddListener(() => MainController.ReturnHome());

        _homeUI.SetActive(true);
        _gameUI.SetActive(false);
    }

    void OnStartNewGame()
    {
        foreach(var life in _lifes)
        {
            life.SetActive(true);
        }
        _imgColorToRun.color = Color.white;
        _homeUI.SetActive(false);
        _gameUI.SetActive(true);
        _currentScore = 0;
        _txtScoreValue.text = _currentScore.ToString();
        _currentLife = 4;
        MainController.PlaySound(SoundType.Background);
    }

    void OnReturnHome()
    {
        _homeUI.SetActive(true);
        _gameUI.SetActive(false);
        _popupEnd.SetActive(false);
        MainController.StopSound(SoundType.Background);
    }

    void UpdateScore(int scoreUpdate)
    {
        _currentScore += scoreUpdate;
        _txtScoreValue.text = _currentScore.ToString();
    }

    void UpdateLife()
    {
        _currentLife--;
        _lifes[_currentLife].SetActive(false);
    }

    void UpdateColor(ColorType t)
    {
        _imgColorToRun.color = AssetManager.GetBlockColor(t);
    }

    void ShowPopupEndGame()
    {
        _popupEnd.SetActive(true);
    }

    void CloseApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the Unity Editor
#else
            Application.Quit(); // Quit the application in a built player
#endif
    }
}

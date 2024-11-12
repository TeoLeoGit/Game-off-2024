using System;

public class MainController
{
    public static event Action<SoundType> OnPlaySound;
    public static event Action<SoundType> OnStopSound;
    public static event Action OnEndGame;
    public static event Action OnPlayGame;
    public static event Action OnShowHome;
    public static event Action OnUpdateLifeView;

    public static void PlaySound(SoundType soundType)
    {
        OnPlaySound?.Invoke(soundType);
    }

    public static void EndGame()
    {
        OnEndGame?.Invoke();
    }

    public static void StartGame()
    {
        OnPlayGame?.Invoke();

        GameController.CallStartMap(MainModel.CurrentMapId, 1);
    }

    public static void ShowHome()
    {
        OnShowHome?.Invoke();
    }

    public static void UpdateLifeView()
    {
        OnUpdateLifeView?.Invoke();
    }

    public static void StopSound(SoundType soundType)
    {
        OnStopSound?.Invoke(soundType);
    }
}

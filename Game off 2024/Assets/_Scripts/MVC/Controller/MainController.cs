using System;

public class MainController
{
    public static event Action<SoundType> OnPlaySound;
    public static event Action<SoundType> OnStopSound;
    public static event Action OnEndGame;
    public static event Action OnNewGame;
    public static event Action OnReturnHome;
    public static event Action OnUpdateLifeView;

    public static void PlaySound(SoundType soundType)
    {
        OnPlaySound?.Invoke(soundType);
    }

    public static void EndGame()
    {
        OnEndGame?.Invoke();
    }

    public static void StartNewGame()
    {
        OnNewGame?.Invoke();
    }

    public static void ReturnHome()
    {
        OnReturnHome?.Invoke();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioClip _clipWalk;
    [SerializeField] AudioClip _clipHurt;
    [SerializeField] AudioClip _clipBackground;
    [SerializeField] AudioClip _clipHome;
    [SerializeField] AudioClip _clipExplode;


    [SerializeField] AudioClip _clipWin;

    [SerializeField] GameObject _audioSource;

    private AudioSource _bgSource;

    private void Awake()
    {
        MainController.OnPlaySound += PlaySound;
        MainController.OnStopSound += StopSound;

    }

    private void OnDestroy()
    {
        MainController.OnPlaySound -= PlaySound;
        MainController.OnStopSound -= StopSound;

    }

    private void PlaySound(SoundType type)
    {
        var audioSource = Instantiate(_audioSource).GetComponent<AudioSource>();

        switch (type)
        {
            case SoundType.Walk:
                audioSource.clip = _clipWalk;
                break;
            case SoundType.Hurt:
                audioSource.clip = _clipHurt;
                break;
            case SoundType.Explode:
                audioSource.clip = _clipExplode;
                break;
            case SoundType.Win:
                audioSource.clip = _clipWin;
                break;
            case SoundType.Background:
                _bgSource = audioSource;
                audioSource.loop = true;
                audioSource.clip = _clipBackground;
                break;
            case SoundType.Home:
                audioSource.loop = true;
                audioSource.clip = _clipHome;
                break;
        }
        audioSource.Play();
        StartCoroutine(IDestroySource(audioSource));
    }

    IEnumerator IDestroySource(AudioSource s)
    {
        yield return null;
        yield return null;
        yield return null;

        yield return new WaitUntil(() => s.isPlaying == false);
        Destroy(s.gameObject);
    }

    void StopSound(SoundType type)
    {
        switch (type)
        {
            case SoundType.Background:
                Destroy(_bgSource.gameObject);
                break;
        }
    }
}


public enum SoundType
{
    Walk = 1,
    Background = 2,
    Win = 3,
    Home = 4,
    Hurt = 5,
    Explode = 6,
}

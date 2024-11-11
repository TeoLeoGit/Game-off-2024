using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioData _data;
    [SerializeField] private GameObject _audioSource;

    private Dictionary<SoundType, AudioRecord> _mapSound = new();
    private Dictionary<SoundType, AudioSource> _mapSource = new();

    private void Awake()
    {
        MainController.OnPlaySound += PlaySound;
        MainController.OnStopSound += StopSound;
    }

    private void Start()
    {
        foreach(var record in _data.AudioRecords)
        {
            _mapSound.Add(record.clipName, record);
        }
    }

    private void OnDestroy()
    {
        MainController.OnPlaySound -= PlaySound;
        MainController.OnStopSound -= StopSound;
    }

    private void PlaySound(SoundType type)
    {
        AudioSource audioSource;
        if (_mapSource.ContainsKey(type)) 
        {
            audioSource = _mapSource[type];
        }
        else
        {
            audioSource = Instantiate(_audioSource).GetComponent<AudioSource>();
            if (_mapSound[type].isLoop) _mapSource.Add(type, audioSource);
        }
        audioSource.clip = _mapSound[type].audioClip;
        audioSource.loop = _mapSound[type].isLoop;

        if (!audioSource.isPlaying)
            audioSource.Play();
        if (!audioSource.loop)
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
        if (_mapSource.ContainsKey(type))
        {
            _mapSource[type].Stop();
        }
    }
}




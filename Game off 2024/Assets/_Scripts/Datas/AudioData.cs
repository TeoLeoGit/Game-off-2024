using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioData", menuName = "ScriptableObjects/AudioData")]
public class AudioData : ScriptableObject
{
    [SerializeField] private List<AudioRecord> _audioRecords;
    public List<AudioRecord> AudioRecords {  get { return _audioRecords; } }
}

[Serializable]
public class AudioRecord
{
    public SoundType clipName;
    public AudioClip audioClip;
    public bool isLoop;
}

public enum SoundType
{
    Walk = 1,
    Background = 2,
    Win = 3,
    Home = 4,
    BossChase = 6
}
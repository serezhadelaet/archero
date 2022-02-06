using System;
using Levels;
using UnityEngine;

[CreateAssetMenu]
public class NewWaveEventSo : ScriptableObject
{
    public event Action<LevelSettings.WaveSettings> Event;

    public void Invoke(LevelSettings.WaveSettings waveSettings)
    {
        Event?.Invoke(waveSettings);
    }
}
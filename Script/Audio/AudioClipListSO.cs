using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClipListSO", menuName = "SO/Audio/ClipListSO")]
public class AudioClipListSO : ScriptableObject
{
    public List<AudioClipDataSO> audioClipDataSOList;

    private Dictionary<EAudioName, AudioClipDataSO> _soundDictonary;

    private void OnEnable()
    {
        _soundDictonary = new Dictionary<EAudioName, AudioClipDataSO>();
        foreach (AudioClipDataSO sound in audioClipDataSOList)
        {
            _soundDictonary.Add(sound.audioName, sound);
        }
    }

    public AudioClipDataSO GetAudioClipData(EAudioName audioEnum)
    {
        return _soundDictonary[audioEnum];
    }
}

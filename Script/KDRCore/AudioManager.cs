using AYellowpaper.SerializedCollections;
using Doryu.JBSave;
using System;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSaveData : ISavable<VolumeSaveData>
{
    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    public float this[EAudioType eAudioType]
    {
        get
        {
            switch (eAudioType)
            {
                case EAudioType.Master:
                    return masterVolume;
                case EAudioType.BGM:
                    return bgmVolume;
                case EAudioType.SFX:
                    return sfxVolume;
                default:
                    return 0;
            }
        }
    }

    public bool ChangeVolume(EAudioType eAudioType, float volume)
    {
        bool flag = false;
        switch (eAudioType)
        {
            case EAudioType.Master:
                if (masterVolume != volume)
                {
                    masterVolume = volume;
                    flag = true;
                }
                break;
            case EAudioType.BGM:
                if (bgmVolume != volume)
                {
                    bgmVolume = volume;
                    flag = true;
                }
                break;
            case EAudioType.SFX:
                if (sfxVolume != volume)
                {
                    sfxVolume = volume;
                    flag = true;
                }
                break;
            default:
                break;
        }
        return flag;
    }

    public void OnLoadData(VolumeSaveData classData)
    {
        masterVolume = classData.masterVolume;
        bgmVolume = classData.bgmVolume;
        sfxVolume = classData.sfxVolume;
    }

    public void OnSaveData(string savedFileName)
    {

    }

    public void ResetData()
    {
        masterVolume = 1f;
        bgmVolume = 0.5f;
        sfxVolume = 0.5f;
    }
}

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioClipListSO _audios;
    [SerializeField] private AudioPlayer _soundPlayerPrefab;

    public VolumeSaveData VolumeSaveData { get; private set; }
    private Dictionary<EAudioType, float> _volumeDictionary = new Dictionary<EAudioType, float>();


    public event Action<VolumeSaveData> OnVolumeChanged;

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);

        VolumeSaveData = new VolumeSaveData();
        bool onLoad = VolumeSaveData.LoadJson("SoundVolume");
        if (onLoad == false) VolumeSaveData.ResetData();

        DontDestroyOnLoad(gameObject);
    }

    public void SetVolume(EAudioType eAudioType, float volume)
    {
        bool flag = VolumeSaveData.ChangeVolume(eAudioType, volume);
        
        if (flag)
        {
            VolumeSaveData.SaveJson("SoundVolume");
            OnVolumeChanged?.Invoke(VolumeSaveData);
        }
    }

    public AudioPlayer PlaySound(EAudioName soundEnum, Transform parent)
    {
        AudioPlayer soundPlayer = Instantiate(_soundPlayerPrefab, parent);
        soundPlayer.transform.localPosition = Vector3.zero;
        soundPlayer.Init(_audios.GetAudioClipData(soundEnum));
        return soundPlayer;
    }
    public AudioPlayer PlaySound(EAudioName soundEnum, Vector3 pos)
    {
        AudioPlayer soundPlayer = Instantiate(_soundPlayerPrefab);
        soundPlayer.transform.position = pos;
        soundPlayer.Init(_audios.GetAudioClipData(soundEnum));
        return soundPlayer;
    }
    public AudioPlayer PlaySound(EAudioName soundEnum, Transform parent, Vector3 pos)
    {
        AudioPlayer soundPlayer = Instantiate(_soundPlayerPrefab, parent);
        soundPlayer.transform.localPosition = pos;
        soundPlayer.Init(_audios.GetAudioClipData(soundEnum));
        return soundPlayer;
    }

    public void StopSound(EAudioName soundEnum, Transform target = null)
    {
        AudioPlayer[] soundPlayers;
        if (target == null)
            soundPlayers = FindObjectsByType<AudioPlayer>(FindObjectsSortMode.None);
        else
            soundPlayers = target.GetComponentsInChildren<AudioPlayer>();

        AudioClipDataSO sound = _audios.GetAudioClipData(soundEnum);

        for (int i = 0; i < soundPlayers.Length; i++)
        {
            if (soundPlayers[i].audioName == sound.audioName)
                soundPlayers[i].Die();
        }
    }
    public void StopSound(AudioPlayer soundPlayer)
    {
        soundPlayer.Die();
    }
}

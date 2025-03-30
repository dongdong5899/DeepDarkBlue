using UnityEngine;

[System.Serializable]
public enum EAudioName
{
    TitleBGM,
    InGameBGM,
    FlashLightTurnOn,
    PlayerHit,
    EnemyHit,
    Click,
    Bubble,
    WeaponAttack,
    Bullet,
    FlashBang,
    BossBGM,
    BigBite,
    Bite,
    Hit,
    Breath,
}
[System.Serializable]
public enum EAudioType
{
    Master,
    BGM,
    SFX
}

[CreateAssetMenu(fileName = "ClipDataSO", menuName = "SO/Audio/ClipData")]
public class AudioClipDataSO : ScriptableObject
{
    public EAudioName audioName;
    public EAudioType audioType;
    [Range(0f, 1f)]
    public float volume = 0.5f;
    public float duration;
    [Tooltip("������ �Ѹ� Duration�� ������� �ʽ��ϴ�.")]
    public bool isLoop = false;
    public bool is3D = true;
    public bool isDonDestroy;
    public AudioClip clip;
}

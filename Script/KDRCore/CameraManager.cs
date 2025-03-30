using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private SerializedDictionary<string, CinemachineCamera> _cameraDictionary = new SerializedDictionary<string, CinemachineCamera>();


    private CinemachineVirtualCameraBase _currentCamera;
    public CinemachineVirtualCameraBase currentCamera 
    {
        get
        {
            if (_currentCamera == null)
            {
                CinemachineVirtualCameraBase currentCam = CinemachineCore.GetVirtualCamera(0);
                _currentCamera = currentCam;
            }

            return _currentCamera;
        }
        private set
        {
            _currentCamera = value;
            currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }

    private CinemachineBasicMultiChannelPerlin _currentMultiChannel;
    private CinemachineBasicMultiChannelPerlin currentMultiChannel
    {
        get 
        {
            if (_currentMultiChannel == null)
                _currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            return _currentMultiChannel;
        }
        set => _currentMultiChannel = value;
    }


    public void ChangeCamera(CinemachineCamera camera)
    {
        currentCamera.Priority = 0;
        currentCamera = camera;
        currentCamera.Priority = 1;
        currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void ChangeCamera(string cameraName)
    {
        currentCamera.Priority = 0;
        currentCamera = _cameraDictionary[cameraName];
        currentCamera.Priority = 1;
        currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private Sequence _shakeSequence;
    public void ShakeCamera(float amplitude, float frequency, float time, AnimationCurve curve)
    {
        if (_shakeSequence != null && _shakeSequence.IsActive()) _shakeSequence.Kill();
        _shakeSequence = DOTween.Sequence();

        _shakeSequence
            .Append(
                DOTween.To(() => amplitude,
                value => currentMultiChannel.AmplitudeGain = value,
                0, time).SetEase(curve))
            .Join(
                DOTween.To(() => frequency,
                value => currentMultiChannel.FrequencyGain = value,
                0, time).SetEase(curve));
    }
    public void ShakeCamera(float amplitude, float frequency, float time, Ease ease = Ease.Linear)
    {
        if (_shakeSequence != null && _shakeSequence.IsActive()) _shakeSequence.Kill();
        _shakeSequence = DOTween.Sequence();

        _shakeSequence
            .Append(
                DOTween.To(() => amplitude,
                value => currentMultiChannel.AmplitudeGain = value,
                0, time).SetEase(ease))
            .Join(
                DOTween.To(() => frequency,
                value => currentMultiChannel.FrequencyGain = value,
                0, time).SetEase(ease));
    }
}

using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DepthGlodalLight : MonoBehaviour
{
    [SerializeField] private Light2D _light2D;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _yArea;
    [SerializeField] private Vector2 _minMaxIntensity;
    [SerializeField] private AnimationCurve _curve;

    

    private void Update()
    {
        if (_target == null) return;
        float amount = Mathf.Clamp((_target.position.y - _yArea.x) / (_yArea.y - _yArea.x), 0, 1);
        _light2D.intensity = Mathf.Lerp(_minMaxIntensity.x, _minMaxIntensity.y, _curve.Evaluate(amount));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(0, (_yArea.x + _yArea.y) / 2, 0), new Vector3(300, Mathf.Abs(_yArea.x - _yArea.y), 0));
    }
}

using UnityEngine;

public class OmnidirectionalFlashUpAction : EquipAction
{
    private float _prevOuterRadius;
    private float _prevAngle;
    public override void Equip()
    {
        _prevOuterRadius = GameManager.Instance.Player.FlashLight.Light2D.pointLightOuterRadius;
        _prevAngle = GameManager.Instance.Player.FlashLight.Light2D.pointLightOuterAngle;
        GameManager.Instance.Player.SetFlash(20, 2, _prevOuterRadius, 360);
    }

    public override void UnEquip()
    {
        GameManager.Instance.Player.SetFlash(10, 1, _prevOuterRadius, _prevAngle);
    }
}

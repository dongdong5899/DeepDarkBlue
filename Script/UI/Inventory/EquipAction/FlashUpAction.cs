using UnityEngine;

public class FlashUpAction : EquipAction
{
    private float _prevOuterRadius;
    private float _prevAngle;
    public override void Equip()
    {
        _prevOuterRadius = GameManager.Instance.Player.FlashLight.Light2D.pointLightOuterRadius;
        _prevAngle = GameManager.Instance.Player.FlashLight.Light2D.pointLightOuterAngle;
        GameManager.Instance.Player.SetFlash(10, 3, _prevOuterRadius * 1.5f, _prevAngle);
    }

    public override void UnEquip()
    {
        GameManager.Instance.Player.SetFlash(10, 1, _prevOuterRadius, _prevAngle);
    }
}

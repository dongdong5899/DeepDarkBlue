using UnityEngine;

public class SpeedUpAction : EquipAction
{
    public override void Equip()
    {
        GameManager.Instance.Player.SetSpeedMultiplier(1.3f);
    }

    public override void UnEquip()
    {
        GameManager.Instance.Player.SetSpeedMultiplier(1f);
    }
}

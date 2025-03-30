using UnityEngine;

public class DefenseUpAction : EquipAction
{
    public override void Equip()
    {
        GameManager.Instance.Player.SetDefense(30f);
    }

    public override void UnEquip()
    {
        GameManager.Instance.Player.SetDefense(0f);
    }
}

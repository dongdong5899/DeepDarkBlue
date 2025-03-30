using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    private List<Weapon> _weapon = new List<Weapon>();
    private Dictionary<ItemSO, Weapon> _weaponDict = new Dictionary<ItemSO, Weapon>();

    private Weapon _currentWeapon;
    public Slot SelectedSlot { get; private set; }

    private void Awake()
    {
        GetComponentsInChildren(_weapon);
        foreach (Weapon weapon in _weapon)
        {
            _weaponDict.Add(weapon.itemSO, weapon);
            weapon.gameObject.SetActive(false);
        }
    }

    public void SelectWeapon(int slotIndex)
    {
        if (SelectedSlot != null) SelectedSlot.OnSlotChangedEvent -= SetWeapon;
        Slot newSlot = InventoryManager.Instance.GetSlot(EInventory.Quick, slotIndex);
        if (SelectedSlot == newSlot)
        {
            SelectedSlot = null;
            SetWeapon(null);
        }
        else
        {
            SelectedSlot = newSlot;
            SelectedSlot.OnSlotChangedEvent += SetWeapon;
            SetWeapon(SelectedSlot.GetAssignedItem());
        }
    }

    public void SetWeapon(Item item)
    {
        if (_currentWeapon != null) _currentWeapon.gameObject.SetActive(false);

        if (item == null)
            _currentWeapon = null;
        else
        {
            _currentWeapon = _weaponDict[item.itemSO];
            _currentWeapon.gameObject.SetActive(true);
        }
    }

    public void Attack()
    {
        if (_currentWeapon != null)
        {
            bool onUse = _currentWeapon.Use();
            if (onUse)
                SelectedSlot.GetAssignedItem().Use();
        }
    }
}

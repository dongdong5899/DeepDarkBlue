using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemList", menuName = "SO/Item/ItemList")]
public class ItemListSO : ScriptableObject
{
    public List<ItemSO> ItemList;
}

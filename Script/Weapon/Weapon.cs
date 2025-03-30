using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public ItemSO itemSO;

    public abstract bool Use();
}

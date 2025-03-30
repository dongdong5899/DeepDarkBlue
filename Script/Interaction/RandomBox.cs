using UnityEngine;

public class RandomBox : MonoBehaviour, IInteractable
{
    public Transform Transform { get => transform; set { } }

    [SerializeField] private GameObject interactionObj;
    [SerializeField] private Vector2Int _minMaxCount;
    [SerializeField] private ParticleSystem _openParticle;
    [SerializeField] private DropItem _dropItemPrefab;
    [SerializeField] private ItemSO[] _itemSOs;

    public void Interaction()
    {
        int index = Random.Range(0, _itemSOs.Length);
        int count = Random.Range(_minMaxCount.x, _minMaxCount.y + 1);
        ItemSO itemSO = _itemSOs[index];
        DropItem dropItem = Instantiate(_dropItemPrefab, transform.position, Quaternion.identity);
        dropItem.Init(itemSO, count);
        ParticleSystem openParticle = Instantiate(_openParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void SetInteractable(bool isInteractable)
    {
        interactionObj.SetActive(isInteractable);
    }
}

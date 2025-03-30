using System;
using System.Collections;
using UnityEngine;

public class ReSourceSpawn : MonoBehaviour
{
    [SerializeField] private int count =1;
    [SerializeField] private GameObject spawnKind;
    [SerializeField] private float spawnDelay = 3f; // ���� ���� ���� �ð�
    [SerializeField] private float respawnCooldown = 5f; // ���� ��� �� ����� ��� �ð�
    [SerializeField] private ItemSO _itemSo;
    [SerializeField] private GameObject _currentThing;
    [SerializeField] private Transform _transform;
    [SerializeField] private bool isFlip;
    private DropItem _dropItem;

    private bool isAroundPlayer = false;
    private float playerFarTime = 0;
    private bool isSpawning = false; // �ߺ� ���� ���� �÷���
    private bool isCooldown = false; // ��� �� ��� ���� �÷���

    private void Awake()
    {
        _transform = transform;
    }

    void Start()
    {
        if (_itemSo != null)
        {
            _dropItem = spawnKind.GetComponent<DropItem>();
            _dropItem.Init(_itemSo, count);
        }
    }

    public IEnumerator SpawnMonster()
    {
        // ���� ���� �� ���
        yield return new WaitForSeconds(spawnDelay);

        // ���� ���� (�ߺ� ���� Ȯ��)
        if (_currentThing == null)
        {
            _currentThing = Instantiate(spawnKind, _transform.position, isFlip ? Quaternion.Euler(0, 180, 0) : Quaternion.identity);
        }

        isSpawning = false;
    }

    private IEnumerator RespawnCooldown()
    {
        isCooldown = true;

        // ��� �� ��� �ð�
        yield return new WaitForSeconds(respawnCooldown);

        isCooldown = false;
    }

    private void Update()
    {
        // �÷��̾� ����
        isAroundPlayer = Physics2D.OverlapCircle(_transform.position, 30f, LayerMask.GetMask("Player"));

        // ���Ͱ� ����, �÷��̾ ��ó�� �ְ�, ��� ���°� �ƴ� ��� ���� ����
        if (_currentThing == null && isAroundPlayer && !isSpawning && !isCooldown)
        {
            isSpawning = true;
            StartCoroutine(SpawnMonster());
            playerFarTime = 0;
        }

        // �÷��̾ ������ ����� �� ���� ����
        if (!isAroundPlayer && _currentThing != null)
        {
            playerFarTime += Time.deltaTime;
            if (playerFarTime > 10)
            {
                Destroy(_currentThing);
                StartCoroutine(RespawnCooldown()); // ���� ���� �� ��� ���·� ��ȯ
            }
        }
    }
}
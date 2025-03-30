using System;
using System.Collections;
using UnityEngine;

public class ReSourceSpawn : MonoBehaviour
{
    [SerializeField] private int count =1;
    [SerializeField] private GameObject spawnKind;
    [SerializeField] private float spawnDelay = 3f; // 몬스터 생성 지연 시간
    [SerializeField] private float respawnCooldown = 5f; // 몬스터 사망 후 재생성 대기 시간
    [SerializeField] private ItemSO _itemSo;
    [SerializeField] private GameObject _currentThing;
    [SerializeField] private Transform _transform;
    [SerializeField] private bool isFlip;
    private DropItem _dropItem;

    private bool isAroundPlayer = false;
    private float playerFarTime = 0;
    private bool isSpawning = false; // 중복 생성 방지 플래그
    private bool isCooldown = false; // 사망 후 대기 상태 플래그

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
        // 몬스터 생성 전 대기
        yield return new WaitForSeconds(spawnDelay);

        // 몬스터 생성 (중복 방지 확인)
        if (_currentThing == null)
        {
            _currentThing = Instantiate(spawnKind, _transform.position, isFlip ? Quaternion.Euler(0, 180, 0) : Quaternion.identity);
        }

        isSpawning = false;
    }

    private IEnumerator RespawnCooldown()
    {
        isCooldown = true;

        // 사망 후 대기 시간
        yield return new WaitForSeconds(respawnCooldown);

        isCooldown = false;
    }

    private void Update()
    {
        // 플레이어 감지
        isAroundPlayer = Physics2D.OverlapCircle(_transform.position, 30f, LayerMask.GetMask("Player"));

        // 몬스터가 없고, 플레이어가 근처에 있고, 대기 상태가 아닌 경우 몬스터 생성
        if (_currentThing == null && isAroundPlayer && !isSpawning && !isCooldown)
        {
            isSpawning = true;
            StartCoroutine(SpawnMonster());
            playerFarTime = 0;
        }

        // 플레이어가 범위를 벗어났을 때 몬스터 삭제
        if (!isAroundPlayer && _currentThing != null)
        {
            playerFarTime += Time.deltaTime;
            if (playerFarTime > 10)
            {
                Destroy(_currentThing);
                StartCoroutine(RespawnCooldown()); // 몬스터 삭제 후 대기 상태로 전환
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [field: SerializeField] public InputReaderSO InputReader { get; private set; }
    [field: SerializeField] public WeaponHolder WeaponHolder { get; private set; }
    [field: SerializeField] public FlashLight FlashLight { get; private set; }
    [SerializeField] private ParticleSystem _defaultParticle;
    [SerializeField] private ParticleSystem _sprintParticle;
    private Animator flashLightAnimator;
    [Header("Speed")]
    public float acceleration = 5f; // ���ӵ�
    public float defaultSpeed = 3f; // �ִ� �ӵ�
    public float sprintSpeed = 3f; // �ִ� �ӵ�
    private float speed = 3f; // �ִ� �ӵ�
    private float speedMultiplier = 1f; // �ִ� �ӵ�
    public float drag = 2f; // �� �ӿ����� ����

    private bool _canMove;

    public bool IsDie { get; private set; }
    private float defense = 1f;
    public int curHp;
    [Header("Health")]
    public int maxHp = 60;

    private float curStamina;
    [Header("Stamina")]
    [SerializeField] private float maxStamina = 10;
    [SerializeField] private float staminaDownSpeed = 2;
    [SerializeField] private float staminaUpSpeed = 1;

    private float _curFlashLight;
    [Header("FlashLight")]
    [SerializeField] private float _maxFlashLight = 5f;
    [SerializeField] private float _flashLightDownSpeed = 1f;
    [SerializeField] private float _flashLightUpSpeed = 1f;

    [Space(10)]
    [SerializeField] private int[] safeDepthLevels;

    public event Action<int, int> OnHealthChangedEvent;
    public event Action OnDieEvent;

    private Rigidbody2D rigid;

    private bool isGrab;
    private bool _isOnFlash;
    private bool _isSprint;

    private Dictionary<EInventory, EquipAction> _equipActionDict;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        flashLightAnimator = FlashLight.GetComponent<Animator>();

        curHp = maxHp;
        _curFlashLight = _maxFlashLight;
        speed = defaultSpeed;
        curStamina = maxStamina;

        _equipActionDict = new Dictionary<EInventory, EquipAction>()
        {
            { EInventory.HeadArmor, null },
            { EInventory.BodyArmor, null },
            { EInventory.FootArmor, null }
        };
    }

    public void SetInput()
    {
        _canMove = true;
        InputReader.OnQuickSlotEvent += HandleQuickSlotEvent;
        InputReader.OnFireEvent += HandleFireEvent;
        InputReader.OnFlashEvent += HandleFlashEvent;
        InputReader.OnSprintEvent += HandleSprintEvent;
    }

    private void Start()
    {
        if (InventoryManager.Instance != null)
        {
            StartCoroutine(ApplyWaterPressure());

            InventoryManager.Instance.AddChangeListener(EInventory.HeadArmor, (index, item) => HandleArmorSlotChangedEvent(EInventory.HeadArmor, item));
            InventoryManager.Instance.AddChangeListener(EInventory.BodyArmor, (index, item) => HandleArmorSlotChangedEvent(EInventory.BodyArmor, item));
            InventoryManager.Instance.AddChangeListener(EInventory.FootArmor, (index, item) => HandleArmorSlotChangedEvent(EInventory.FootArmor, item));
        }
    }

    private void HandleArmorSlotChangedEvent(EInventory eInventory, Item item)
    {
        _equipActionDict[eInventory]?.UnEquip();
        _equipActionDict[eInventory] = item == null ? null : item.itemSO.equipAction;
        _equipActionDict[eInventory]?.Equip();
    }

    private void OnDestroy()
    {
        InputReader.OnQuickSlotEvent -= HandleQuickSlotEvent;
        InputReader.OnFireEvent -= HandleFireEvent;
        InputReader.OnFlashEvent -= HandleFlashEvent;
        InputReader.OnSprintEvent -= HandleSprintEvent;
    }

    private IEnumerator ApplyWaterPressure()
    {
        while (true)
        {
            Item bodyArmor = InventoryManager.Instance.GetSlot(EInventory.BodyArmor, 0).GetAssignedItem();
            int safeDepth = GetSafeDepth(bodyArmor);
            int depth = (-(int)(GameManager.Instance.PlayerTrm.position.y - 50) * 100);

            if (depth > safeDepth)
            {
                int damage = Mathf.RoundToInt((depth - safeDepth) * 0.001f);
                curHp -= damage;

                TakeDamage(curHp / maxHp);
                AudioManager.Instance.PlaySound(EAudioName.Breath, transform, transform.position);
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    private int GetSafeDepth(Item item)
    {
        if (item == null)
        {
            return safeDepthLevels[0];
        }

        switch (item.itemSO.itemID)
        {
            case EItemID.NormalPressureShut:
                return safeDepthLevels[1];
            case EItemID.HighPressureShut:
                return safeDepthLevels[2];
            default:
                return 0;
        }
    }

    private void HandleSprintEvent(bool isOn)
    {
        if (_isSprint == isOn) return;

        _isSprint = isOn;
        if (isOn)
        {
            AudioManager.Instance.PlaySound(EAudioName.Bubble, transform, transform.position);
            speed = sprintSpeed;
            _defaultParticle.Stop();
            _sprintParticle.Play();
        }
        else
        {
            speed = defaultSpeed;
            _defaultParticle.Play();
            _sprintParticle.Stop();
        }
    }

    private void HandleFlashEvent(bool isOn)
    {
        if (GameManager.Instance.IsFlashToggle)
        {
            if (isOn)
                LightToggle(!_isOnFlash);
        }
        else
        {
            LightToggle(isOn);
        }
    }

    private void HandleFireEvent()
    {
        if (UIManager.Instance.IsOpenedUI(ETogleUIName.Inventory) ||
            UIManager.Instance.IsOpenedUI(ETogleUIName.ESC) ||
            UIManager.Instance.IsOpenedUI(ETogleUIName.Option))
            return;
        WeaponHolder?.Attack();
    }

    private void HandleQuickSlotEvent(int index)
    {
        WeaponHolder.SelectWeapon(index);
    }

    private void Update()
    {
        // 수압 시스템
        ApplyWaterPressure();

        if (_isOnFlash && _curFlashLight > 0)
        {
            _curFlashLight -= Time.deltaTime * _flashLightDownSpeed;
        }
        else if (!_isOnFlash && _curFlashLight < _maxFlashLight)
        {
            _curFlashLight += Time.deltaTime * _flashLightUpSpeed;
        }
        _curFlashLight = Mathf.Clamp(_curFlashLight, 0, _maxFlashLight);
        UIManager.Instance.SetLightBatteryAmount(_curFlashLight / _maxFlashLight);
        if (_isOnFlash && _curFlashLight == 0)
        {
            StartCoroutine(Discharge());
        }


        if (_isSprint && curStamina > 0)
        {
            curStamina -= Time.deltaTime * staminaDownSpeed;
        }
        else if (!_isSprint && curStamina < maxStamina)
        {
            curStamina += Time.deltaTime * staminaUpSpeed;
        }
        curStamina = Mathf.Clamp(curStamina, 0, maxStamina);
        UIManager.Instance.SetStaminaAmount(curStamina / maxStamina);
        if (_isSprint && curStamina == 0)
        {
            HandleSprintEvent(false);
        }
    }

    private void FixedUpdate()
    {
        if (isGrab)
        {
            rigid.linearVelocity = Vector3.zero;
            return; // ���ĸ����� ������ �� ������ ����
        }

        if (_canMove == false) return;

        Vector2 targetVelocity = InputReader.Movement * speed * speedMultiplier;
        Vector2 velocityDiff = targetVelocity - rigid.linearVelocity;

        Vector2 force = velocityDiff * acceleration;
        rigid.AddForce(force);
    }

    public void SetSpeedMultiplier(float value)
    {
        speedMultiplier = value;
    }

    public void SetDefense(float percent)
    {
        defense = 1 - percent / 100;
    }

    public void SetFlash(float maxFlashLight, float upSpeed, float outerRadius, float angle)
    {
        FlashLight.Light2D.pointLightOuterRadius = outerRadius;
        FlashLight.Light2D.pointLightOuterAngle = angle;
        FlashLight.Light2D.pointLightInnerAngle = angle;
        _maxFlashLight = maxFlashLight;
        _flashLightUpSpeed = upSpeed;
    }

    public void GrabStateToggle(bool on)
    {
        isGrab = on;
    }

    public void LightToggle(bool on)
    {
        _isOnFlash = on;

        if (on)
        {
            AudioManager.Instance.PlaySound(EAudioName.FlashLightTurnOn, transform, transform.position);
        }
        FlashLight.transform.gameObject.SetActive(_isOnFlash);
    }

    public void DischargeAnimation()
    {
        flashLightAnimator.SetTrigger("Discharge");
    }

    public void TakeDamage(int damage)
    {
        int prev = curHp;
        curHp -= Mathf.CeilToInt(damage * defense);
        curHp = Mathf.Clamp(curHp, 0, maxHp);
        UIManager.Instance.SetHealthAmount((float)curHp / maxHp);
        AudioManager.Instance.PlaySound(EAudioName.PlayerHit, transform, transform.position);

        OnHealthChangedEvent?.Invoke(prev, curHp);
        if (curHp == 0 && IsDie == false)
        {
            IsDie = true;
            UIManager.Instance.Die();
            OnDieEvent?.Invoke();

            Fade.Out(0.5f, () => SceneManager.LoadScene("Main"));
        }
    }

    private IEnumerator Discharge()
    {
        WaitForSeconds waitForSeconds03 = new WaitForSeconds(0.3f);
        WaitForSeconds waitForSeconds01 = new WaitForSeconds(0.1f);

        FlashLight.transform.gameObject.SetActive(false);
        yield return waitForSeconds03;
        FlashLight.transform.gameObject.SetActive(true);
        yield return waitForSeconds01;
        FlashLight.transform.gameObject.SetActive(false);
        yield return waitForSeconds01;
        FlashLight.transform.gameObject.SetActive(true);
        yield return waitForSeconds01;
        FlashLight.transform.gameObject.SetActive(false);
        yield return waitForSeconds03;
        _isOnFlash = false;
    }
}

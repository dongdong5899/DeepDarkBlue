
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Level
{
    public string pointName;
    public GameObject spawnY; // �÷��̾ �� ���� ������ ������
    public UnityEvent actionEvent;
}

public class PlaceManager : MonoBehaviour
{
    private int level; // �� �������� ������ ������
    [SerializeField] private Level[] levelArray;
    // ���Ͱ� ������ ���̰��� ����

    [SerializeField] private TMP_Text txt_deepLength; // ���� �ؽ�Ʈ
    [SerializeField] private TMP_Text txt_pointName;
    [SerializeField] private SpriteRenderer _bg;
    [SerializeField] private Color[] _color;
    [SerializeField] private float _fadeSpeed ;
    private int _coloridx;
    private Animator textAnimator;

    private void Awake()
    {
        _coloridx = 0;
       textAnimator = txt_pointName.transform.GetComponent<Animator>();
    }

    private void Update()
    {
        if (level >= levelArray.Length)
            return;

        if(GameManager.Instance.PlayerTrm.position.y < levelArray[level].spawnY.transform.position.y)
        {
            //SpawnNextMonster();
            NextLevelAnimation(levelArray.Length, levelArray[level].pointName);
            level++;
        }
    }

    public void SpawnNextMonster()
    {
        //Instantiate(levelArray[level].monsterSpawners[], levelArray[level].spawnPoint.position, Quaternion.identity);
    }

    private void NextLevelAnimation(int length, string name)
    {
        txt_pointName.text = name;
        txt_deepLength.text = UIManager.Instance.txt_km.text;
        //txt_deepLength.text =length.ToString() + "km";
        textAnimator.SetTrigger("Play");
        levelArray[level].actionEvent?.Invoke();
    }

    public void ChangeToColorBG()
    {
        StartCoroutine(ChangeColorFlow(_color[_coloridx]));
    }

    private IEnumerator ChangeColorFlow(Color color)
    {
        for (var i = 0f; i < 3f; i += Time.deltaTime)
        {
            _bg.color=Color.Lerp(_bg.color, color, _fadeSpeed * Time.deltaTime);
            yield return null;
        }

        if (!(_coloridx + 1 > _color.Length))
        {
            _coloridx++;
        }
        
    }
}
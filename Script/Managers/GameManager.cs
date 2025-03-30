// # System
using Doryu.JBSave;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class TogleSaveData : ISavable<TogleSaveData>
{
    public bool isFlashToggle = false;

    public void OnLoadData(TogleSaveData classData)
    {
        isFlashToggle = classData.isFlashToggle;
    }

    public void OnSaveData(string savedFileName)
    {

    }

    public void ResetData()
    {
        isFlashToggle = false;
    }
}

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField]
    private ItemListSO   itemListSO;
    private List<ItemSO> itemList;

    public Transform PlayerTrm {  get; private set; }
    public Player Player {  get; private set; }
    public bool IsFlashToggle => ToggleSaveData.isFlashToggle;

    public TogleSaveData ToggleSaveData { get; private set; }

    private int stage;

    private void Awake()
    {
        Player = FindFirstObjectByType<Player>();
        PlayerTrm = Player.transform;

        itemList = itemListSO.ItemList;

        ToggleSaveData = new TogleSaveData();
        if (ToggleSaveData.LoadJson("FlashTogle") == false)
        {
            ToggleSaveData.ResetData();
        }
    }

    private void Start()
    {
        AudioManager.Instance.PlaySound(EAudioName.InGameBGM, PlayerTrm, PlayerTrm.position);
    }

    private void Update()
    {

    }

    public void SetToggle(bool isTogleOn)
    {
        ToggleSaveData.isFlashToggle = isTogleOn;
        ToggleSaveData.SaveJson("FlashTogle");
    }

    public ItemSO GetItemList()
    {
        int number = Random.Range(0, itemList.Count);

        return itemList[number];
    }
}

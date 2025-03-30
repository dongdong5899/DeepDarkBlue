using UnityEngine;

public class ActiveTogleUI : TogleUI
{
    public override void Init(bool isOpenStart)
    {
        gameObject.SetActive(isOpenStart);
    }

    public override void Open()
    {
        base.Open();
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        base.Close();
        gameObject.SetActive(false);
    }
}

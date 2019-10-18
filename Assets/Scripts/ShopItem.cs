using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour {

    public enum ItemType
    {
        FIRST_SKIN,
        SECOND_SKIN
    }

    public ItemType Type;
    public Button BuyBtn, ActivateBtn;
    public bool IsBought;
    public int Cost;

    bool IsActive
    {
        get
        {
            return Type == SM.ActiveSkin;
        }
    }

    GameManager gm;
    public ShopManager SM;

    public void Init()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void CheckButtons()
    {
        BuyBtn.gameObject.SetActive(!IsBought);
        BuyBtn.interactable = CanBuy();

        ActivateBtn.gameObject.SetActive(IsBought);
        ActivateBtn.interactable = !IsActive;
    }

    bool CanBuy()
    {
        return gm.Coins >= Cost;
    }

    public void BuyItem()
    {
        if (!CanBuy())
            return;

        IsBought = true;
        gm.Coins -= Cost;

        CheckButtons();

        SaveManager.Instance.SaveGame();
        gm.RefreshText();
    }

    public void ActivateItem()
    {
        SM.ActiveSkin = Type;
        SM.CheckItemButtons();

        switch (Type)
        {
            case ItemType.FIRST_SKIN:
                gm.ActivateSkin(0, true);
                break;
            case ItemType.SECOND_SKIN:
                gm.ActivateSkin(1, true);
                break;
        }

        SaveManager.Instance.SaveGame();
    }

}

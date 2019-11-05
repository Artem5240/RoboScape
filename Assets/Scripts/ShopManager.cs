using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

    public List<ShopItem> Items;  // Список всех предметов
    public ShopItem.ItemType ActiveSkin;  // текущий активный скин

    public void OpenShop()  // Включение объекта с магазином
    {
        CheckItemButtons();
        gameObject.SetActive(true);
    }

    public void CloseShop()     // Отключение объекта с магазином
    {
        gameObject.SetActive(false);
    }

    public void CheckItemButtons()
    {
        foreach (ShopItem item in Items)
        {
            item.SM = this;
            item.Init();
            item.CheckButtons();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour {

    public enum ItemType  // Enum со скинами
    {
        FIRST_SKIN,
        SECOND_SKIN
    }

    public ItemType Type;
    public Button BuyBtn, ActivateBtn; // переменные для кнопок покупки и активации
    public bool IsBought;   // Куплен ли предмет?
    public int Cost;        // Стоимость

    bool IsActive
    {
        get
        {
            return Type == SM.ActiveSkin;  // Сравниваем тип текущего активного скина с типом предмета
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
        BuyBtn.gameObject.SetActive(!IsBought);   // Если объект не куплен
        BuyBtn.interactable = CanBuy();           // Объект кнопки активен

        ActivateBtn.gameObject.SetActive(IsBought);   // Если предмет куплен, то кнока активации активна
        ActivateBtn.interactable = !IsActive;         // Кнопка кликабильна если предмет со скином не активен
    }

    bool CanBuy()         // проверка хватает ли игроку денег на покупку
    {
        return gm.Coins >= Cost;
    }

    public void BuyItem()
    {
        if (!CanBuy())   // если не можем купить то выходим
            return;

        IsBought = true;  // Флаг покупки предмета
        gm.Coins -= Cost;  // Списание монет

        CheckButtons();

        SaveManager.Instance.SaveGame();  // Сохранение игры после покупки
        gm.RefreshText();  // Отображение кол-ва монет после покупки предмета
    }

    public void ActivateItem()
    {
        SM.ActiveSkin = Type;     // Передаем в ShopManager тип текущего скина
        SM.CheckItemButtons();    // обновляем кнопки на всех предметах

        switch (Type)   // В зависимости от типа активируем скин с определенным индексом
        {
            case ItemType.FIRST_SKIN:
                gm.ActivateSkin(0, true);
                break;
            case ItemType.SECOND_SKIN:
                gm.ActivateSkin(1, true);
                break;
        }

        SaveManager.Instance.SaveGame(); // Сохранение игры после активации 
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public PauseMenuController PMC;
    public GameObject ResultObj;
    public PlayerMovement PM;
    public RoadSpawner RS;

    public Text PointsTxt,
                CoinsTxt;
    float Points;

    public int Coins = 0;

    public bool CanPlay = true;
    public bool IsSound = true;

    public float BaseMoveSpeed, CurrentMoveSpeed;
    public float PointsBaseValue, PointsMultiplier, PowerUpMultiplier;      //исход. значение получаемых очков в сек.

    public List<Skin> Skins;        // Список скинов

    public void StartGame()
    {
        PM.Respawn();
        ResultObj.SetActive(false);
        RS.StartGame();
        CanPlay = true;

        PM.SkinAnimator.SetTrigger("respawn");
        StartCoroutine(FixTrigger());

        CurrentMoveSpeed = BaseMoveSpeed;   // багфикс: очки не обнуляются и скорость не восстанавливается
        PointsMultiplier = 1;
        PowerUpMultiplier = 1;
        Points = 0;
    }

    IEnumerator FixTrigger()     // Для работы респавна после добавления меню
    {
        yield return null;
        PM.SkinAnimator.ResetTrigger("respawn");
    }

    private void Update()
    {
        if (CanPlay)
        {
            if (Input.GetKeyDown(KeyCode.Escape))   // Если нажат Escape то вызываем меню паузы
                PMC.Pause();

            Points += PointsBaseValue * PointsMultiplier * PowerUpMultiplier * Time.deltaTime;  // Начисление очков

            PointsMultiplier += .05f * Time.deltaTime;          // Увеличение на 0,05 каждую секунду
            PointsMultiplier = Mathf.Clamp(PointsMultiplier, 1, 10); // Ограничиваем в диапазоне от 1 до 10 чтобы прирост не был бесконечным

            CurrentMoveSpeed += .1f * Time.deltaTime;     // Увеличение скорости на 0,1 каждую секунду
            CurrentMoveSpeed = Mathf.Clamp(CurrentMoveSpeed, 1, 20); // ограничиваем от 1 до 20
        }

        PointsTxt.text = ((int)Points).ToString();   // Отображение очков
    }

    public void ShowResult()
    {
        ResultObj.SetActive(true);
        SaveManager.Instance.SaveGame();  // Сохранение, т.к. метод вызывается при смерти игрока
    }

    public void AddCoins(int number)        // Добавление монет и отображение их кол-ва
    {
        Coins += number;
        RefreshText();
    }

    public void RefreshText()    // Ф-ция отображения кол-ва монет
    {
        CoinsTxt.text = Coins.ToString();
    }

    public void ActivateSkin(int skinIndex, bool setTrigger = false)    // Возможность активировать скины
    {
        foreach (var skin in Skins)  // Сначала прячем все скины
            skin.HideSkin();

        Skins[skinIndex].ShowSkin();        // Показываем скин по нашему индексу
        PM.SkinAnimator = Skins[skinIndex].AC;

        if (setTrigger) // триггер чтоб сразу не проигрывалась анимация сметри
            PM.SkinAnimator.SetTrigger("death");  // Вызов триггера смерти чтобы после смены скина анимации работали нормально
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameManager GM;
    public MainMenuController MMC;
    public PlayerMovement PM;

    public void Pause()                 // Кнопка паузы
    {
        gameObject.SetActive(true);   // Включаем объект меню паузы
        GM.CanPlay = false;
        PM.Pause();
    }

    public void Resume()            // Кнопка продолжения
    {
        gameObject.SetActive(false);   // Отключаем меню паузы
        GM.CanPlay = true;
        PM.UnPause();
    }

    public void MenuBtn()   // Кнопка Main menu
    {
        PM.UnPause();
        PM.PUController.ResetAllPowerUps();

        gameObject.SetActive(false);
        MMC.OpenMenu();
    }
}
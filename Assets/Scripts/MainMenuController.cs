using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameManager GM;

    public Sprite SoundsOn, SoundsOff;  // Спрайты кнопки звука
    public Image SoundsBtnImg;          // Изображение кнопки звука

    public void PlayBtn()     // Метод для кнопки плей
    {
        gameObject.SetActive(false);  // Отклчаем объект меню
        GM.StartGame();
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }

    public void QuitBtn()           // Метод для кнопки выход
    {
        Application.Quit();
    }

    public void SoundBtn()          // Метод для кнопки звука
    {
        GM.IsSound = !GM.IsSound;
        SoundsBtnImg.sprite = GM.IsSound ? SoundsOn : SoundsOff;  // Обновляем спрайт кнопки в зависимости от IsSound
        AudioManager.Instance.RefreshSoundState();
    }
}
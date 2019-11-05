using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerupScr : MonoBehaviour
{
    public Image Progressbar; // Изображение прогрессбара
    public Color[] Colors;  // Массив цветов для каждого паверапа

    public void SetData(PowerUpController.PowerUp.Type type)  // Принимает на вход тип паверапа
    {
        Progressbar.color = Colors[(int)type];              // И устанавливает цвет в зависимости от типа
    }

    public void SetProgress(float progress)
    {
        Progressbar.fillAmount = progress;
    }

    public void Destroy()       // Уничтожаем геймобджект
    {
        Destroy(gameObject);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {

    public struct PowerUp
    {
        public enum Type
        {
            MUILTIPLIER,   // Мультиплаер очков
            IMMORTALITY,    // Бессмертие
            COINS_SPAWN     // Спавн монет на всех участках пути
        }
        public Type PowerUpType;
        public float Duration;
    }

    public delegate void OnCoinsPowerUp(bool activate);       // Для паверапа на монетки
    public static event OnCoinsPowerUp CoinsPowerUpEvent;     // Для паверапа на монетки

    PowerUp[] powerUps = new PowerUp[3];            // Массив паверапов
    Coroutine[] powerUpsCors = new Coroutine[3];    // Масиив корутин (по одной на каждый тип паверапа)

    public GameManager GM;
    public PlayerMovement PM;

    public GameObject PowerupPref;   // Префаб паверапов
    public Transform PowerupGrid;
    List<PowerupScr> powerups = new List<PowerupScr>();  //  Список паверапов с геймобджектами на блоке пути

	void Start ()
    {
        powerUps[0] = new PowerUp() { PowerUpType = PowerUp.Type.MUILTIPLIER, Duration = 8 };   // Инициализация массива
        powerUps[1] = new PowerUp() { PowerUpType = PowerUp.Type.IMMORTALITY, Duration = 5 };   // Паверапов
        powerUps[2] = new PowerUp() { PowerUpType = PowerUp.Type.COINS_SPAWN, Duration = 7 };

        PlayerMovement.PowerUpUseEvent += PowerUpUse;
    }

    void PowerUpUse(PowerUp.Type type)
    {
        PowerUpReset(type);   // Чтоб при поднятии 2х одинаковых паверапов подряд эффекты не накладывались друг на друга
        powerUpsCors[(int)type] = StartCoroutine(PowerUpCor(type, CreatePowerupPref(type)));

        switch (type)    // Свич для применения эффекта паверапа
        {
            case PowerUp.Type.MUILTIPLIER:
                GM.PowerUpMultiplier = 2;    // Удвоение очков
                break;
            case PowerUp.Type.IMMORTALITY:
                PM.ImmortalityOn();         // Включение бессмертия
                break;
            case PowerUp.Type.COINS_SPAWN:
                if (CoinsPowerUpEvent != null)
                    CoinsPowerUpEvent(true);
                break;
        }
    }

    void PowerUpReset(PowerUp.Type type)
    {
        if (powerUpsCors[(int)type] != null)        // Если корутина с данным типом паверапа запущена
            StopCoroutine(powerUpsCors[(int)type]);   // Останавливаем ее
        else                    // Иначе выходим из функции
            return;

        powerUpsCors[(int)type] = null;    // Обнуляем корутину по индексу

        switch (type)  // Свич для деактивации эффекта паверапа
        {
            case PowerUp.Type.MUILTIPLIER:
                GM.PowerUpMultiplier = 1;   // Возвращение к норме
                break;
            case PowerUp.Type.IMMORTALITY:
                PM.ImmortalityOff();      // Отключене бессмертия
                break;
            case PowerUp.Type.COINS_SPAWN:
                if (CoinsPowerUpEvent != null)
                    CoinsPowerUpEvent(false);
                break;
        }
    }

    public void ResetAllPowerUps()     // Остановка всех действующих эффектов
    {
        for (int i = 0; i < powerUps.Length; i++)   // Проходимся по массиву паверапов
            PowerUpReset(powerUps[i].PowerUpType);  // И вызываем ресет с типом каждого паверапа

        foreach (var pu in powerups)
            pu.Destroy();

        powerups.Clear();
    }

    IEnumerator PowerUpCor(PowerUp.Type type, PowerupScr powerupPref)   // Контроль длительности действия паверапов
    {
        float duration = powerUps[(int)type].Duration;
        float currDuration = duration;

        while (currDuration > 0)
        {
            powerupPref.SetProgress(currDuration / duration);

            if (GM.CanPlay)                     // Чтобы во время паузы не уменьшалось время паверапов
                currDuration -= Time.deltaTime;     // Уменьшение времени паверапов

            yield return null;
        }

        powerups.Remove(powerupPref);   // Удаляем паверап из списка
        powerupPref.Destroy();

        PowerUpReset(type);
    }

    PowerupScr CreatePowerupPref(PowerUp.Type type)   // Инстанциируем префаб и возвращем созданный паверап
    {
        GameObject go = Instantiate(PowerupPref, PowerupGrid, false);

        var ps = go.GetComponent<PowerupScr>();

        powerups.Add(ps);

        ps.SetData(type);
        return ps;
    }

}
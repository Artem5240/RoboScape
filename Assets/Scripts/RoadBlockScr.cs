using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockScr : MonoBehaviour {

    GameManager GM;
    Vector3 moveVec;

    public GameObject CoinsObj;   // объект для монеток

    public int CoinChance;    // Шанс на спавн монеток: устанавливаем в юнити от 0 до 100 (тип проценты)
    bool coinsSpawn;
    bool powerUpSpawn;

    public List<GameObject> PowerUps;

	void Start ()
    {
        PowerUpController.CoinsPowerUpEvent += CoinsEvent;

        GM = FindObjectOfType<GameManager>();
        moveVec = new Vector3(-1, 0, 0);

        coinsSpawn = Random.Range(0, 101) <= CoinChance;  // Если ранд число от 0 до 100 =< шансу
        CoinsObj.SetActive(coinsSpawn);                   // Объект с монетками активен

        powerUpSpawn = Random.Range(0, 101) <= 10 && !coinsSpawn;
        if (powerUpSpawn)
            PowerUps[Random.Range(0, PowerUps.Count)].SetActive(true);   // Активируем рандомный паверап из списка
    }
	
	void Update ()     // Если можем играть то перемещаем игрока
    {
        if (GM.CanPlay)
            transform.Translate(moveVec * Time.deltaTime * GM.CurrentMoveSpeed);
	}

    void CoinsEvent(bool activate)
    {
        if (activate)
        {
            CoinsObj.SetActive(true);       // Включаем объект монет
            return;
        }

        if (!coinsSpawn)     // Если монеты не были заспавнены изначально то отключаем их
            CoinsObj.SetActive(false);  
    }

    private void OnDestroy()      // Фикс ошибки при удалении монет
    {
        PowerUpController.CoinsPowerUpEvent -= CoinsEvent;
    }
}

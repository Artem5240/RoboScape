using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockScr : MonoBehaviour {

    GameManager GM;
    Vector3 moveVec;

    public GameObject CoinsObj;

    public int CoinChance;
    bool coinsSpawn;
    bool powerUpSpawn;

    public List<GameObject> PowerUps;

	void Start ()
    {
        PowerUpController.CoinsPowerUpEvent += CoinsEvent;

        GM = FindObjectOfType<GameManager>();
        moveVec = new Vector3(-1, 0, 0);

        coinsSpawn = Random.Range(0, 101) <= CoinChance;
        CoinsObj.SetActive(coinsSpawn);

        powerUpSpawn = Random.Range(0, 101) <= 10 && !coinsSpawn;
        if (powerUpSpawn)
            PowerUps[Random.Range(0, PowerUps.Count)].SetActive(true);
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
            CoinsObj.SetActive(true);
            return;
        }

        if (!coinsSpawn)
            CoinsObj.SetActive(false);
    }

    private void OnDestroy()
    {
        PowerUpController.CoinsPowerUpEvent -= CoinsEvent;
    }
}

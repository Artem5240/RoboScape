using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {

    public struct PowerUp
    {
        public enum Type
        {
            MUILTIPLIER,
            IMMORTALITY,
            COINS_SPAWN
        }
        public Type PowerUpType;
        public float Duration;
    }

    public delegate void OnCoinsPowerUp(bool activate);
    public static event OnCoinsPowerUp CoinsPowerUpEvent;

    PowerUp[] powerUps = new PowerUp[3];
    Coroutine[] powerUpsCors = new Coroutine[3];

    public GameManager GM;
    public PlayerMovement PM;

    public GameObject PowerupPref;
    public Transform PowerupGrid;
    List<PowerupScr> powerups = new List<PowerupScr>(); 

	void Start ()
    {
        powerUps[0] = new PowerUp() { PowerUpType = PowerUp.Type.MUILTIPLIER, Duration = 8 };
        powerUps[1] = new PowerUp() { PowerUpType = PowerUp.Type.IMMORTALITY, Duration = 5 };
        powerUps[2] = new PowerUp() { PowerUpType = PowerUp.Type.COINS_SPAWN, Duration = 7 };

        PlayerMovement.PowerUpUseEvent += PowerUpUse;
    }

    void PowerUpUse(PowerUp.Type type)
    {
        PowerUpReset(type);
        powerUpsCors[(int)type] = StartCoroutine(PowerUpCor(type, CreatePowerupPref(type)));

        switch (type)
        {
            case PowerUp.Type.MUILTIPLIER:
                GM.PowerUpMultiplier = 2;
                break;
            case PowerUp.Type.IMMORTALITY:
                PM.ImmortalityOn();
                break;
            case PowerUp.Type.COINS_SPAWN:
                if (CoinsPowerUpEvent != null)
                    CoinsPowerUpEvent(true);
                break;
        }
    }

    void PowerUpReset(PowerUp.Type type)
    {
        if (powerUpsCors[(int)type] != null)
            StopCoroutine(powerUpsCors[(int)type]);
        else
            return;

        powerUpsCors[(int)type] = null;

        switch (type)
        {
            case PowerUp.Type.MUILTIPLIER:
                GM.PowerUpMultiplier = 1;
                break;
            case PowerUp.Type.IMMORTALITY:
                PM.ImmortalityOff();
                break;
            case PowerUp.Type.COINS_SPAWN:
                if (CoinsPowerUpEvent != null)
                    CoinsPowerUpEvent(false);
                break;
        }
    }

    public void ResetAllPowerUps()
    {
        for (int i = 0; i < powerUps.Length; i++)
            PowerUpReset(powerUps[i].PowerUpType);

        foreach (var pu in powerups)
            pu.Destroy();

        powerups.Clear();
    }

    IEnumerator PowerUpCor(PowerUp.Type type, PowerupScr powerupPref)
    {
        float duration = powerUps[(int)type].Duration;
        float currDuration = duration;

        while (currDuration > 0)
        {
            powerupPref.SetProgress(currDuration / duration);

            if (GM.CanPlay)
                currDuration -= Time.deltaTime;

            yield return null;
        }

        powerups.Remove(powerupPref);
        powerupPref.Destroy();

        PowerUpReset(type);
    }

    PowerupScr CreatePowerupPref(PowerUp.Type type)
    {
        GameObject go = Instantiate(PowerupPref, PowerupGrid, false);

        var ps = go.GetComponent<PowerupScr>();

        powerups.Add(ps);

        ps.SetData(type);
        return ps;
    }

}
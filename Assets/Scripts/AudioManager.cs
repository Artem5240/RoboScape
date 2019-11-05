using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public GameManager GM;

    public AudioSource BGAS, EffectAS;  // Переменные для источника звука
    public AudioClip CoinSnd;           // Переменная для звука монет

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RefreshSoundState()   // Пауза и продолжение проигрывания музыки
    {
        if (GM.IsSound)
            BGAS.UnPause();
        else
            BGAS.Pause();
    }

    public void PlayCoinEffect()
    {
        if (GM.IsSound)
            EffectAS.PlayOneShot(CoinSnd);
    }
}
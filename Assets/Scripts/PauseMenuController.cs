using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameManager GM;
    public MainMenuController MMC;
    public PlayerMovement PM;

    public void Pause()
    {
        gameObject.SetActive(true);
        GM.CanPlay = false;
        PM.Pause();
    }

    public void Resume()
    {
        gameObject.SetActive(false);
        GM.CanPlay = true;
        PM.UnPause();
    }

    public void MenuBtn()
    {
        PM.UnPause();
        PM.PUController.ResetAllPowerUps();

        gameObject.SetActive(false);
        MMC.OpenMenu();
    }
}
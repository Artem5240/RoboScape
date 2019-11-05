using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin : MonoBehaviour {

    public Animator AC;

    public void ShowSkin()   // Активация скина
    {
        gameObject.SetActive(true);
    }

    public void HideSkin()  // Деактивация скина
    {
        gameObject.SetActive(false);
    }

}

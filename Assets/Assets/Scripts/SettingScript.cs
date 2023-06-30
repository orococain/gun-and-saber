using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingScript : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource soundEffects;
   // public GameObject screenShakeObject;
    //public Light flashLight;

    private bool isMusicOn = true;
    private bool isSoundOn = true;
    private bool isScreenShakeOn = true;
    private bool isFlashlightOn = true;

    private void Start()
    {
        // Khởi tạo trạng thái ban đầu
        backgroundMusic.Play();
        soundEffects.Play();
       // screenShakeObject.SetActive(true);
      //  flashLight.enabled = true;
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;

        if (isMusicOn)
            backgroundMusic.Play();
        else
            backgroundMusic.Stop();
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;

        if (isSoundOn)
            soundEffects.volume = 1f;
        else
            soundEffects.volume = 0f;
    }

    // public void ToggleScreenShake()
    // {
    //     isScreenShakeOn = !isScreenShakeOn;
    //
    //     screenShakeObject.SetActive(isScreenShakeOn);
    // }
    //
    // public void ToggleFlashlight()
    // {
    //     isFlashlightOn = !isFlashlightOn;
    //
    //     flashLight.enabled = isFlashlightOn;
    // }
}

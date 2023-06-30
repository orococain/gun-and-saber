using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaberSelect : MonoBehaviour
{
    public GameObject[] saber;
    public int currentSaberIndex = -1;
    public Image switchSaberImage1;
    public Image switchSaberImage2;
    public Image switchSaberImage3;
    public Image switchSaberImage4;
    public Image switchSaberImage5;
    public Image switchSaberImage6;
    public Image switchSaberImage7;
    public Image switchSaberImage8;
    private int previoussaberIndex = -1;
    public GameObject uiShopSaber;
    void Start()
    {
        // Add event listeners to switch gun images
        switchSaberImage1.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(0));
        switchSaberImage2.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(1));
        switchSaberImage3.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(2));
        switchSaberImage4.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(3));
        switchSaberImage5.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(4));
        switchSaberImage6.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(5));
        switchSaberImage7.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(6));
        switchSaberImage8.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(7));
        // Disable all guns except the first one
        for (int i = 0; i < saber.Length; i++)
        {
            if (i != currentSaberIndex)
            {
                saber[i].SetActive(false);
            }
        }
    }

    public void SwitchToGun(int index)
    {
        if (index >= saber.Length || index < 0 || index == currentSaberIndex)
        {
            return;
        }

        previoussaberIndex = currentSaberIndex;
        currentSaberIndex = index;

        if ( previoussaberIndex >= 0 &&  previoussaberIndex < saber.Length) // tắt model cũ trước khi kích hoạt model mới0
        {
            saber[ previoussaberIndex].SetActive(false);
        }
        for (int i = 0; i < saber.Length; i++)
        {
            saber[i].SetActive(false);
        }
        saber[currentSaberIndex].SetActive(true);
        uiShopSaber.SetActive(false);
    }
    
    public void DisableCurrentSaber()
    {
        if (currentSaberIndex >= 0 && currentSaberIndex < saber.Length) // tắt model cũ trước khi kích hoạt model mới
        {
            saber[currentSaberIndex].SetActive(false);
        }

    }
}

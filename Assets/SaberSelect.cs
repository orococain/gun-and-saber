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
        switchSaberImage7.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(7));
        switchSaberImage8.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(8));
        // Disable all guns except the first one
        for (int i = 0; i < saber.Length; i++)
        {
            if (i != currentSaberIndex)
            {
                saber[i].SetActive(false);
            }
        }
    }

    void SwitchToGun(int index)
    {
        if (index >= saber.Length || index < 0 || index == currentSaberIndex)
        {
            return;
        }

        if (currentSaberIndex >= 0 && currentSaberIndex < saber.Length)
        {
            saber[currentSaberIndex].SetActive(false);
        }

        currentSaberIndex = index;
        saber[currentSaberIndex].SetActiveRecursively(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScifiGunSelect : MonoBehaviour
{
    public GameObject[] guns;
    public int currentGunIndex = -1;
    public Image switchGunImage1;
    public Image switchGunImage2;
    public Image switchGunImage3;
    public Image switchGunImage4;
    public Image switchGunImage5;
    public Image switchGunImage6;
    public Image switchGunImage7;
    public Image switchGunImage8;
    
    void Start()
    {
        // Add event listeners to switch gun images
        switchGunImage1.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(0));
        switchGunImage2.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(1));
        switchGunImage3.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(2));
        switchGunImage4.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(3));
        switchGunImage5.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(4));
        switchGunImage6.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(5));
        switchGunImage7.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(6));
        switchGunImage8.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(7));
        // Disable all guns except the first one
        for (int i = 0; i < guns.Length; i++)
        {
            if (i != currentGunIndex)
            {
                guns[i].SetActive(false);
            }
        }
    }

    void SwitchToGun(int index)
    {
        if (index >= guns.Length || index < 0 || index == currentGunIndex)
        {
            return;
        }

        if (currentGunIndex >= 0 && currentGunIndex < guns.Length)
        {
            guns[currentGunIndex].SetActive(false);
        }

        currentGunIndex = index;
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(false);
        }
        guns[currentGunIndex].SetActive(true);
    }
}


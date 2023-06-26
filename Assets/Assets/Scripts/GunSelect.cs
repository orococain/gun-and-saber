using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunSelect : MonoBehaviour
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
    public Image switchGunImage9;
    public Image switchGunImage10;
    private int previousGunIndex = -1;
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
        switchGunImage9.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(8));
        switchGunImage10.GetComponent<Button>().onClick.AddListener(() => SwitchToGun(9));
        // Disable all guns except the first one
        for (int i = 0; i < guns.Length; i++)
        {
            if (i != currentGunIndex)
            {
                guns[i].SetActive(false);
            }
        }
    }

  public  void SwitchToGun(int index)
    {
        if (index >= guns.Length || index < 0 || index == currentGunIndex)
        {
            return;
        }

        previousGunIndex = currentGunIndex;
        currentGunIndex = index;
        for (int i = 0; i < guns.Length; i++)
        {
            guns[i].SetActive(false);
        }
        guns[currentGunIndex].SetActive(true);
    }

  public void DisableCurrentGun()
  {
      if (previousGunIndex >= 0 && previousGunIndex < guns.Length) // tắt model cũ trước khi kích hoạt model mới
      {
          guns[currentGunIndex].SetActive(false);
      }
  }
}


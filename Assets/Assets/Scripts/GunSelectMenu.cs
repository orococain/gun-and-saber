using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GunSelectMenu : MonoBehaviour
{ 
    public GameObject[] guns;
    public int currentGunIndex = -1;
    public Button[] switchGunButtons;
    public GameObject UIShop;
    public GameObject UIGamePlay;
    private int previousGunIndex = -1; // Lưu danh sách vũ khí trước đó trước khi chuyển đổi
    private GunSelect gunSelect; 

    void Start()
    {
        // Add event listeners to switch gun buttons
        for (int i = 0; i < switchGunButtons.Length; i++)
        {
            int index = i;
            switchGunButtons[i].onClick.AddListener(() => SwitchToGun(index));
        }

        // Disable all guns except the first one
        for (int i = 0; i < guns.Length; i++)
        {
            if (i != currentGunIndex)
            {
                guns[i].SetActive(false);
            }
        }
    }

    public void SwitchToGun(int index)
    {
        if (index >= guns.Length || index < 0 || index == currentGunIndex)
        {
            return;
        }

        previousGunIndex = currentGunIndex; // Lưu danh sách vũ khí trước đó
        currentGunIndex = index;
    
        DeactivatePreviousGun(); // Tắt model trước khi kích hoạt model mới

        guns[currentGunIndex].SetActive(true);

        UIShop.SetActive(false);
        UIGamePlay.SetActive(true);
    }
    
    private void DeactivatePreviousGun()
    {
        if (previousGunIndex >= 0 && previousGunIndex < guns.Length && guns[previousGunIndex].activeSelf)
        {
            guns[previousGunIndex].SetActive(false);
        }
    }
}

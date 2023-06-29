using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VapeSelectMenu : MonoBehaviour
{
    public GameObject[] vapes;
    public int currentVapeIndex = -1;
    public Button[] switchVapeButtons;
    public GameObject UIShop;
    public GameObject UIGamePlay; 
    public GameObject UIGameMode;
    void Start()
    {
        // Add event listeners to switch gun buttons
        for (int i = 0; i < switchVapeButtons.Length; i++)
        {
            int index = i;
            switchVapeButtons[i].onClick.AddListener(() => SwitchToVape(index));
        }

        // Disable all vapes except the first one
        for (int i = 0; i < vapes.Length; i++)
        {
            if (i != currentVapeIndex)
            {
                vapes[i].SetActive(false);
            }
        }
    }

    public void SwitchToVape(int index)
    {
        if (index >= vapes.Length || index < 0 || index == currentVapeIndex)
        {
            return;
        }

        if (currentVapeIndex >= 0 && currentVapeIndex < vapes.Length)
        {
            vapes[currentVapeIndex].SetActive(false);
        }

        currentVapeIndex = index;
        vapes[currentVapeIndex].SetActive(true);
        UIShop.SetActive(false);
        UIGamePlay.SetActive(true);
        UIGameMode.SetActive(false);
    }
    public void ResetVapeSelection()
    {
        currentVapeIndex = -1;
        for (int i = 0; i < vapes.Length; i++)
        {
            vapes[i].SetActive(false);
        }
    }
}

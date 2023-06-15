using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGrounData : MonoBehaviour
{
   public GameObject[] backgrounds;
    public int currentBackgroundIndex = -1;
    public Image switchBackgroundImage1;
    public Image switchBackgroundImage2;
    public Image switchBackgroundImage3;
    public Image switchBackgroundImage4;
    public Image switchBackgroundImage5;
    public Image switchBackgroundImage6;

    void Start()
    {
        // Add event listeners to switch background images
        switchBackgroundImage1.GetComponent<Button>().onClick.AddListener(() => SwitchToBackground(0));
        switchBackgroundImage2.GetComponent<Button>().onClick.AddListener(() => SwitchToBackground(1));
        switchBackgroundImage3.GetComponent<Button>().onClick.AddListener(() => SwitchToBackground(2));
        switchBackgroundImage4.GetComponent<Button>().onClick.AddListener(() => SwitchToBackground(3));
        switchBackgroundImage5.GetComponent<Button>().onClick.AddListener(() => SwitchToBackground(4));
        switchBackgroundImage6.GetComponent<Button>().onClick.AddListener(() => SwitchToBackground(5));

        // Disable all backgrounds except the first one
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (i != currentBackgroundIndex)
            {
                backgrounds[i].SetActive(false);
            }
        }
    }

    void SwitchToBackground(int index)
    {
        if (index >= backgrounds.Length || index < 0 || index == currentBackgroundIndex)
        {
            return;
        }

        if (currentBackgroundIndex >= 0 && currentBackgroundIndex < backgrounds.Length)
        {
            backgrounds[currentBackgroundIndex].SetActive(false);
        }

        currentBackgroundIndex = index;
        backgrounds[currentBackgroundIndex].SetActiveRecursively(true);
    }
}

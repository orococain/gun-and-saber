using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VapeSelect : MonoBehaviour
{
    public GameObject[] vaPe;
    public int currentvaPeIndex = -1;
    public Image switchvaPeImage1;
    public Image switchvaPeImage2;
    public Image switchvaPeImage3;
    public Image switchvaPeImage4;
    public Image switchvaPeImage5;
    public Image switchvaPeImage6;
    public Image switchvaPeImage7;
    public Image switchvaPeImage8;
    public Image switchvaPeImage9;
    public Image switchvaPeImage10;
    void Start()
    {
        // Add event listeners to switch gun images
        switchvaPeImage1.GetComponent<Button>().onClick.AddListener(() => SwitchToVape(0));
        switchvaPeImage2.GetComponent<Button>().onClick.AddListener(() => SwitchToVape(1));
        switchvaPeImage3.GetComponent<Button>().onClick.AddListener(() => SwitchToVape(2));
        switchvaPeImage4.GetComponent<Button>().onClick.AddListener(() => SwitchToVape(3));
        switchvaPeImage5.GetComponent<Button>().onClick.AddListener(() => SwitchToVape(4));
        switchvaPeImage6.GetComponent<Button>().onClick.AddListener(() => SwitchToVape(5));
        switchvaPeImage7.GetComponent<Button>().onClick.AddListener(() => SwitchToVape(6));
        switchvaPeImage8.GetComponent<Button>().onClick.AddListener(() => SwitchToVape(7));
        switchvaPeImage9.GetComponent<Button>().onClick.AddListener(() => SwitchToVape(8));
        switchvaPeImage10.GetComponent<Button>().onClick.AddListener(() => SwitchToVape(8));
        // Disable all guns except the first one
        for (int i = 0; i < vaPe.Length; i++)
        {
            if (i != currentvaPeIndex)
            {
                vaPe[i].SetActive(false);
            }
        }
    }

    void SwitchToVape(int index)
    {
        if (index >= vaPe.Length || index < 0 || index == currentvaPeIndex)
        {
            return;
        }

        if (currentvaPeIndex >= 0 && currentvaPeIndex < vaPe.Length)
        {
            vaPe[currentvaPeIndex].SetActive(false);
        }

        currentvaPeIndex = index;
        for (int i = 0; i < vaPe.Length; i++)
        {
            vaPe[i].SetActive(false);
        }
        vaPe[currentvaPeIndex].SetActive(true);
    }
}

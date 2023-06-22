using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunc2 : MonoBehaviour
{
    public GameObject bgScrollView;
    public GameObject bgSCrollView2;
    public void ShowBG()
    {
        if (bgScrollView.activeInHierarchy == true)
            bgScrollView.SetActive(false);
        else
        {
            bgScrollView.SetActive(true);
            bgSCrollView2.SetActive(false);
        }
    }
}

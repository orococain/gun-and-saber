using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunc : MonoBehaviour
{

    public GameObject bgScrollView;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShowBG()
    {
        if (bgScrollView.activeInHierarchy == true)
            bgScrollView.SetActive(false);
        else
        {
            bgScrollView.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

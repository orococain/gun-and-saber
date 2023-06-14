using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToGamePlay : MonoBehaviour
{
    public GameObject uiGamePlay;

    public GameObject uiShopGamePlay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void switchUI()
    {
        uiGamePlay.SetActive(true);
        uiShopGamePlay.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonBack : MonoBehaviour
{
    [SerializeField]
    public GameObject backToMenu;
    public GameObject gamePlayUi;

    public GameObject uiMainLobby;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void backTomenu()
    {
        backToMenu.SetActive(true);
        gamePlayUi.SetActive(false);
        uiMainLobby.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

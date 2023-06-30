using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeImage : MonoBehaviour
{

    public Sprite newButtonimage;
    public Sprite oldButtonimage;

    public Button button;
    private bool isActive = false;
    
    public void ChangeButtonImage()
    {
        isActive = !isActive;
        
        if (isActive)
        {
            button.image.sprite = oldButtonimage;
        }
        else
        {
            button.image.sprite = newButtonimage;
        }
   
    }
}



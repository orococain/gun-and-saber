using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Get the first touch
            if (touch.phase == TouchPhase.Began)
            {
        
            }
       
            else if (touch.phase == TouchPhase.Ended)
            {
              
            }
        }
    }
}


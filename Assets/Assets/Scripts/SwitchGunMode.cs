using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGunMode : MonoBehaviour
{
    
    public Gun gunMode;
    
    public void isBurst( )
    {
        gunMode.IsBurst(3);
        Debug.Log("Switch");
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGunMode : MonoBehaviour
{
    
    public List<Gun> guns;

    public void BurstModeAllGuns()
    {
        foreach (Gun gun in guns)
        {
            gun.IsBurst(3);

        }
    }


}

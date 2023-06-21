using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGunMode : MonoBehaviour
{
    
    public List<Gun> guns;
    public List<ScifiGun> gun;
    public void BurstModeAllGuns()
    {
        foreach (Gun gun in guns)
        {
            gun.IsBurst(3);

        }
    }

    public void AutoModeAllGuns()
    {
        foreach (Gun gun in guns)
        {
            gun.isAuto();

        }
    } 
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplayGun : MonoBehaviour
{
    
    [SerializeField]
    public GameObject uiGunShop;

    public GameObject uiMainLobby;

    public GameObject uiShopVape;

    public GameObject uiShopSaber;

    public GameObject uiShopSpecialGun;
    
    public void machineGunClick()
    {
        uiGunShop.SetActive(true);
        uiShopVape.SetActive(false);
        uiMainLobby.SetActive(false);
        uiShopSaber.SetActive(false);
        uiShopSpecialGun.SetActive(false);
    }
    public void SaberClick()
    {
        uiShopSaber.SetActive(true);
        uiShopVape.SetActive(false);
        uiMainLobby.SetActive(false);
        uiGunShop.SetActive(false);
        uiShopSpecialGun.SetActive(false);
    }
    public void specialGunClick()
    {
        uiShopSpecialGun.SetActive(true);
        uiShopVape.SetActive(false);
        uiMainLobby.SetActive(false);
        uiGunShop.SetActive(false);
        uiShopSaber.SetActive(false);
    }
    public void VapeClick()
    {
        uiShopVape.SetActive(true);
        uiMainLobby.SetActive(false);
        uiGunShop.SetActive(false);
        uiShopSaber.SetActive(false);
        uiShopSpecialGun.SetActive(false);
    }

}

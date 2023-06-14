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
    
    public UIGameplayGun.GunInputType currentInputType;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public enum GunInputType
    {
          SINGLE,
          BURST,
          AUTO,
          SHAKE,
    }
    
    public void UpdateBullet(int a )
    {
    }
    public void machineGunClick()
    {
        uiGunShop.SetActive(true);
        uiMainLobby.SetActive(false);
    }
    public void SaberClick()
    {
        uiShopSaber.SetActive(true);
        uiMainLobby.SetActive(false);
    }
    public void specialGunClick()
    {
        uiShopSpecialGun.SetActive(true);
        uiMainLobby.SetActive(false);
    }
    public void VapeClick()
    {
        uiShopVape.SetActive(true);
        uiMainLobby.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public enum GunMode {
    Single,
    Auto,
    Burst
}

public class Gun : MonoBehaviour
{
    [SerializeField] 
    public int bulletMax; //số đạn tối đa của khẩu súng
    public float maxTimePerShot; //thời gian giữa các lần bắn
    public float waitReload; //thời gian chờ để nạp đạn
    public AudioClip clipShoot; //âm thanh khi bắn đạn
    private int numberBurst; //số đạn bắn liên tiếp
    private int burstRest; //số đạn khi bắn burst 
    public AudioClip clipReload; //âm thanh khi nạp đạn
    public Animator anim;
  //  public UIGameplayGun.GunInputType currentInputType; //kiểu nhập liệu cho khẩu súng
    private bool isReloading; //đang trong quá trình nạp đạn
    public int currentBullet; //số đạn hiện tại của khẩu súng
    private UIGameplayGun uiGameplayGun; //đối tượng UI của khẩu súng
    private bool haveBoostEnergy; //đang có boost energy hay không
    public BulletShell BulletShellPrefab; //prefab của hiệu ứng vỏ đạn  khi bắn
    public float BulletShellScale; //tỉ lệ thu nhỏ của hiệu ứng vỏ đạn  khi bắn
    public Transform BulletShellOffset; //vị trí spawn hiệu ứng vỏ đạn  khi bắn
    public float ForceBulletShell; //lực tác động khi spawn hiệu ứng hạt nhựa khi bắn
    public GameObject bullet; //đối tượng hiệu ứng hạt nhựa khi bắn (được spawn từ prefab)
    public Transform PointFx; //vị trí để hiển thị hiệu ứng khi bắn
    public Transform CirclePoint;
    public GameObject MuzzlePrefab; //prefab của hiệu ứng khi bắn
    public float MuzzleScale; //tỉ lệ thu nhỏ của hiệu ứng khi bắn
    public GameObject effect; //prefab của hiệu ứng ánh sáng khi bắn
    public TMP_Text bulletCountText;
    private bool isFiring = false; // Whether the gun is currently firing
    public float fireRate = 0.2f; // The delay between each shot when firing continuously
    private float timeNextShot = 0f; // The time to fire the next sho
    [SerializeField]
    private GunMode currentGunMode = GunMode.Single;
    public void Update()
    {
        if (Input.touchCount> 0 ) 
        {
            OnTouchDown();
        }
        
    }
    public void Start()
    {
        if (bulletCountText != null) {
            bulletCountText.text = currentBullet.ToString();
        }
    }
 
    public void OnTouchDown()
    {
        // Check if the gun is currently firing or not
        // If the gun is already firing, skip this frame, and wait for the next frame
        if (isFiring) {
            return;
        }

        if ( currentBullet > 0) 
        {
            // Mark the gun as firing
            isFiring = true;

            // Shoot the gun for only one bullet at a time
            Shoot();

            // Wait for a delay before the next shot can be fired
            StartCoroutine(FireDelay());
        }
        else
        {
            Reload();
        }
    }
    public void SetSingleMode() {
        currentGunMode = GunMode.Single;
    }

    public void SetAutoMode() {
        currentGunMode = GunMode.Auto;
    }

    public void SetBurstMode() {
        currentGunMode = GunMode.Burst;
    }
    private IEnumerator FireDelay() {
        // Wait for a delay before the next shot can be fired
        yield return new WaitForSeconds(fireRate);

        // Mark the gun as not firing anymore, so the next shot can be fired for the next touch event
        isFiring = false;
    }

    public void Shoot()
    {
        if (currentBullet > 0)
        {
            currentBullet--;
            if (bulletCountText != null)
            {
                bulletCountText.text = currentBullet.ToString();
            }

            SpawnShell();
            gameObject.GetComponent<Animator>().Play("Vector_Shoot");
            AudioSource.PlayClipAtPoint(clipShoot, Camera.main.transform.position);
            SpawnMuzzle();
            if (effect != null)
            {
                GameObject obj = Instantiate(effect, PointFx.position, effect.transform.rotation);
                Destroy(obj, 2.0f);
            }

            if (uiGameplayGun != null)
            {
                uiGameplayGun.UpdateBullet(currentBullet);
            }
            if (currentBullet <= 0 && !isReloading)
            {
                Reload();
            }
        }
    }
    
    public void Reload() {
        {
            if (!isReloading)
            {
                if (currentBullet >= bulletMax) // Kiểm tra súng đã full đạn chưa
                {
                    return;
                }

                if (currentBullet > 0) // Nếu súng chưa hết đạn thì không cho đổi đạn
                {
                    Debug.Log("Still has bullets left!");
                    return;
                }

                isReloading = true;
                gameObject.GetComponent<Animator>().Play("Vector_Reload");
                StartCoroutine(DelayReloadBullet(waitReload));
                PlayMagOutSound();
            } 
        }

    }
    private IEnumerator DelayReloadBullet(float waitTime) {
        yield return new WaitForSeconds(1);
        currentBullet = bulletMax;
        isReloading = false;
        
    }
    public void PlayMagOutSound() {
        AudioSource.PlayClipAtPoint(clipReload, Camera.main.transform.position);
    }

public void PlayMagInSound() {
    if (clipReload != null) {
        AudioSource.PlayClipAtPoint(clipReload, Camera.main.transform.position);
    }
}

public void SpawnShell() {
    if (BulletShellPrefab != null && BulletShellOffset != null) {
            BulletShell shell = Instantiate(BulletShellPrefab, CirclePoint.position, transform.rotation);
        shell.transform.rotation = BulletShellOffset.rotation;

        Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();
        if (shellRigidbody != null) {
            shellRigidbody.AddForce(BulletShellOffset.forward * ForceBulletShell, ForceMode.Impulse);
        }

        Destroy(shell.gameObject, 2.0f);
    }
}

public void SpawnMuzzle() {
    if (MuzzlePrefab != null && PointFx != null)
    {
        GameObject muzzle = Instantiate(MuzzlePrefab, PointFx.position, Quaternion.Euler(new Vector3(180, 0, 180)));
        muzzle.transform.localScale = new Vector3(MuzzleScale, MuzzleScale, MuzzleScale);
        Destroy(muzzle, 2f);
    }
}


}

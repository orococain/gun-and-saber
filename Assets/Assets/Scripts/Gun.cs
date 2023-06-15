using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
    [SerializeField] 
    public int bulletMax; //số đạn tối đa của khẩu súng
    public float waitReload; //thời gian chờ để nạp đạn
    public AudioClip clipShoot; //âm thanh khi bắn đạn
    private int numberBurst; //số đạn bắn liên tiếp
    private int burstRest; //số đạn khi bắn burst 
    public AudioClip clipReload; //âm thanh khi nạp đạn
    private bool isReloading; //đang trong quá trình nạp đạn
    public int currentBullet; //số đạn hiện tại của khẩu súng
    private UIGameplayGun uiGameplayGun; //đối tượng UI của khẩu súng
    public BulletShell BulletShellPrefab; //prefab của hiệu ứng vỏ đạn  khi bắn
    public float BulletShellScale; //tỉ lệ thu nhỏ của hiệu ứng vỏ đạn  khi bắn
    public Transform BulletShellOffset; //vị trí spawn hiệu ứng vỏ đạn  khi bắn
    public float ForceBulletShell; //lực tác động khi spawn hiệu ứng hạt nhựa khi bắn
    public Transform PointFx; //vị trí để hiển thị hiệu ứng khi bắn
    public Transform CirclePoint;
    public GameObject MuzzlePrefab; //prefab của hiệu ứng khi bắn
    public float MuzzleScale; //tỉ lệ thu nhỏ của hiệu ứng khi bắn
    public GameObject effect; //prefab của hiệu ứng ánh sáng khi bắn
    public TMP_Text bulletCountText;
    public Collider2D gunCollider;
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (gunCollider == Physics2D.OverlapPoint(ray.origin)) // Kiểm tra chạm vào Collider bắn súng hay không
            {
                if (currentBullet > 0)
                {
                    FlyBullet();
                }
                else
                {
                    Reload();
                }
            }
        }
    }
    public void Start()
    {
        if (bulletCountText != null) {
            bulletCountText.text = currentBullet.ToString();
        }
    }
    
    public void FlyBullet()
    {
        if (currentBullet > 0)
        {
            currentBullet--;
            if (bulletCountText != null)
            {
                bulletCountText.text = currentBullet.ToString();
            }

            SpawnShell();
            gameObject.GetComponent<Animator>().Play("Shoot");
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
            if (currentBullet <= 0 && isReloading )
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
                gameObject.GetComponent<Animator>().Play("Reload");
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

public void SpawnShell()
{
    if (BulletShellPrefab != null && BulletShellOffset != null)
    {
        BulletShell shell = Instantiate(BulletShellPrefab, CirclePoint.position, BulletShellOffset.rotation);
        Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();
        shellRigidbody.AddForce(BulletShellOffset.right * ForceBulletShell);
        shell.transform.localScale = new Vector3(BulletShellScale, BulletShellScale, BulletShellScale);
        Destroy(shell.gameObject, 2.0f);
    }
}

public void SpawnMuzzle() {
    if (MuzzlePrefab != null && PointFx != null)
    {
        GameObject muzzle = Instantiate(MuzzlePrefab, PointFx.position, Quaternion.Euler(new Vector3(180, 0, 180)), transform.parent);
        muzzle.transform.localScale = new Vector3(MuzzleScale, MuzzleScale, MuzzleScale);
        Destroy(muzzle, 0.3f);
    }
}

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
    [SerializeField] public int bulletMax; //số đạn tối đa của khẩu súng
    public float waitReload; //thời gian chờ để nạp đạn
    public AudioClip clipShoot; //âm thanh khi bắn đạn
    public int numberBurst; //số đạn bắn liên tiếp
    public AudioClip clipReload; //âm thanh khi nạp đạn
    private bool isReloading; //đang trong quá trình nạp đạn
    public int currentBullet; //số đạn hiện tại của khẩu súng
    public BulletShell BulletShellPrefab; //prefab của hiệu ứng vỏ đạn  khi bắn
    public float BulletShellScale; //tỉ lệ thu nhỏ của hiệu ứng vỏ đạn  khi bắn
    public float ForceBulletShell; //lực tác động khi spawn hiệu ứng hạt nhựa khi bắn
    public Transform PointFx; //vị trí để hiển thị hiệu ứng khi bắn
    public Transform CirclePoint;
    public GameObject MuzzlePrefab; //prefab của hiệu ứng khi bắn
    public GameObject effect; //prefab của hiệu ứng ánh sáng khi bắn
    public ParticleSystem MuzzleParticleSystem;
    public TMP_Text bulletCountText;
    public Collider2D gunCollider;
    private bool isFiring = false;
    private bool isSmoking = false;
    private bool isBurst = false;
    private bool isShooting = false; // Thêm biến isShooting để kiểm tra trạng thái bắn liên tục
    private Coroutine shootingCoroutine; // Thêm biến shootingCoroutine để lưu reference của coroutine


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
            if (isFiring) {
                    isFiring = false;
                    StartCoroutine(DelayStopSmoke()); 
                    
            }
            else if (Input.GetMouseButton(0))
            {
                // Nếu người dùng đang giữ chuột trái, thực hiện bắn liên tục
                if (!isBurst || currentBullet > 0)
                {
                    BurstShoot();
                }
            }
        }
    }

    

    public void Start()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = currentBullet.ToString();
        }
    }


    public void isAuto()
    {
        if (currentBullet <= 0 || isShooting) // Nếu hết đạn hoặc đang bắn liên tục thì không thực hiện
            return;
        shootingCoroutine = StartCoroutine(ShootContinuously());
        isShooting = true; // Đánh dấu đang bắn liên tục
    }
    public void StopShooting() // Thêm hàm dừng bắn liên tục
    {
        if (isShooting)
        {
            StopCoroutine(shootingCoroutine);
            isShooting = false; // Đánh dấu không còn bắn liên tục
        }
    }

    public void IsBurst(int bulletNum) 
    {
        if (!isBurst || currentBullet > 0)
        {
            StartCoroutine(BurstShoot(bulletNum));
            Debug.Log("Burst");
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

            isFiring = true;
            if (currentBullet <= 0 && isReloading)
            {
                Reload();
            }

        }
    }
    
    private IEnumerator ShootContinuously()
    {
        while (currentBullet > 0)
        {
            FlyBullet();
            yield return new WaitForSeconds(0.1f); // Chờ 0.1 giây trước khi bắn tiếp
        }
        isShooting = false; // Đánh dấu không còn bắn liên tục khi hết đạn
    }
    
    private IEnumerator BurstShoot(int bulletNum = 3 )
    {
        isFiring = true;
        for (int i = 0; i < numberBurst && currentBullet > 0; i++)
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
            yield return new WaitForSeconds(0.1f);
        }

        if (currentBullet <= 0 && isReloading)
        {
            Reload();
        }

        // Thực hiện chờ giữa các burst
        yield return new WaitForSeconds(0.5f);
        isFiring = false;
    }
    
    

    public void Reload()
    {
        {
            if (!isReloading)
            {
                if (currentBullet >= bulletMax) // Kiểm tra súng đã full đạn chưa
                {
                    return;
                }

                if (currentBullet > 0) // Nếu súng chưa hết đạn thì không cho đổi đạn
                {
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
    

public void SpawnShell()
{
    BulletShell shell = Instantiate(BulletShellPrefab,CirclePoint.position, CirclePoint.rotation);
    Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();
    shellRigidbody.AddForce(CirclePoint.right * ForceBulletShell);
    shell.transform.localScale = new Vector3(BulletShellScale, BulletShellScale, BulletShellScale);
    Destroy(shell.gameObject, 2.0f);
}

public void SpawnMuzzle() {
    if (MuzzlePrefab != null && PointFx != null)
    {
        GameObject muzzle = Instantiate(MuzzlePrefab, PointFx.position, Quaternion.Euler(new Vector3(180, 0, 180)), transform.parent);
        Destroy(muzzle, 0.3f);
    }
}

public void SpawnSmoke()
{
    if (MuzzlePrefab != null && PointFx != null)
    {
        MuzzleParticleSystem.transform.localPosition = Vector3.zero;
        MuzzleParticleSystem.transform.localRotation = Quaternion.identity;
        ParticleSystem ps = MuzzleParticleSystem.GetComponent<ParticleSystem>();
        ps.Play();
    }
}

private IEnumerator DelayStopSmoke()
{
    while (isFiring)
    {
        yield return null;
    }

    if (!isSmoking)
    {
        isSmoking = true;
        SpawnSmoke();
        yield return new WaitForSeconds(MuzzleParticleSystem.main.duration);
        isSmoking = false;
        MuzzleParticleSystem.Stop(); 
    }
}

}

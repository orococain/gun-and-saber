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
    private int numberBurst; //số đạn bắn liên tiếp
    private int burstRest; //số đạn khi bắn burst 
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
    [SerializeField] private bool isBurstMode = false; //chế độ bắn Burst
    [SerializeField] private bool isAutoMode = false; //chế độ bắn tự động
    [SerializeField] private float firingRate = 0.1f; //tần suất bắn trong chế độ bắn tự động

    private Coroutine autoFireCoroutine; //Coroutine của chế độ bắn tự động
    private Coroutine burstFireCoroutine; //Coroutine của chế độ bắn Burst

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
            else
            {
                if (isFiring)
                {
                    isFiring = false;
                    StartCoroutine(DelayStopSmoke());
                }
            }
        }
        else // Kiểm tra xem người chơi có đang bắn hay không
        {
            if (isFiring) // Nếu đang bắn
            {
                isFiring = false;
                StartCoroutine(DelayStopSmoke());
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
    BulletShell shell = Instantiate(BulletShellPrefab, CirclePoint.position, CirclePoint.rotation);
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
        Debug.Log("SMOKE");
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
    [SerializeField] private int bulletMax; //số đạn tối đa của khẩu súng
    [SerializeField] private float waitReload; //thời gian chờ để nạp đạn
    [SerializeField] private AudioClip clipShoot; //âm thanh khi bắn đạn
    [SerializeField] private int numberBurst; //số đạn bắn liên tiếp
    [SerializeField] private AudioClip clipReload; //âm thanh khi nạp đạn
    [SerializeField] private bool isReloading; //đang trong quá trình nạp đạn
    [SerializeField] private int currentBullet; //số đạn hiện tại của khẩu súng
    [SerializeField] private Transform PointFx; //vị trí để hiển thị hiệu ứng khi bắn
    [SerializeField] private Transform CirclePoint;
    [SerializeField] private GameObject MuzzlePrefab; //prefab của hiệu ứng khi bắn
    [SerializeField] private GameObject effect; //prefab của hiệu ứng ánh sáng khi bắn
    [SerializeField] private ParticleSystem MuzzleParticleSystem;
    [SerializeField] private TMP_Text bulletCountText;
    [SerializeField] public Collider2D gunCollider;
    private bool isFiring;
    private bool isSmoking;
    private readonly bool isBurst = false;
    private bool isShooting; // Thêm biến isShooting để kiểm tra trạng thái bắn liên tục
    private Coroutine shootingCoroutine; // Thêm biến shootingCoroutine để lưu reference của coroutine
    private AndroidJavaObject camera;
    private AndroidJavaObject cameraParameters;
    private Queue<GameObject> bulletPool;
    [SerializeField] private int poolSize = 10;
    [SerializeField] private GameObject bulletPrefab;

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
                    ToggleAndroidFlashlight();
                }
                else
                {
                    Reload();
                }
            }

            if (isFiring)
            {
                isFiring = false;
                StartCoroutine(DelayStopSmoke());
            }
            else if (Input.GetMouseButton(0))
            {
                // Nếu người dùng đang giữ chuột trái, thực hiện bắn liên tục
                if (!isBurst || currentBullet > 0) BurstShoot();
            }
        }
    }


    public void Start()
    {
        if (bulletCountText != null) bulletCountText.text = currentBullet.ToString();
        
        bulletPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
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
            if (bulletCountText != null) bulletCountText.text = currentBullet.ToString();
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            bullet.transform.position = CirclePoint.position;
            bullet.transform.rotation = CirclePoint.rotation;
           // SpawnShell();
            gameObject.GetComponent<Animator>().Play("Shoot");
            AudioSource.PlayClipAtPoint(clipShoot, Camera.main.transform.position);
            SpawnMuzzle();
            if (effect != null)
            {
                var obj = Instantiate(effect, PointFx.position, effect.transform.rotation);
                Destroy(obj, 2.0f);
            }
            StartCoroutine(DisableBullet(bullet));

            isFiring = true;
            if (currentBullet <= 0 && isReloading) Reload();
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

    private IEnumerator BurstShoot(int bulletNum = 3)
    {
        isFiring = true;
        for (var i = 0; i < numberBurst && currentBullet > 0; i++)
        {
            currentBullet--;
            if (bulletCountText != null) bulletCountText.text = currentBullet.ToString();

            //SpawnShell();
            gameObject.GetComponent<Animator>().Play("Shoot");
            AudioSource.PlayClipAtPoint(clipShoot, Camera.main.transform.position);
            SpawnMuzzle();
            if (effect != null)
            {
                var obj = Instantiate(effect, PointFx.position, effect.transform.rotation);
                Destroy(obj, 2.0f);
            }

            yield return new WaitForSeconds(0.1f);
        }

        if (currentBullet <= 0 && isReloading) Reload();

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
                    return;

                if (currentBullet > 0) // Nếu súng chưa hết đạn thì không cho đổi đạn
                    return;

                isReloading = true;
                gameObject.GetComponent<Animator>().Play("Reload");
                StartCoroutine(DelayReloadBullet(waitReload));
                PlayMagOutSound();
            }
        }
    }

    private IEnumerator DelayReloadBullet(float waitTime)
    {
        yield return new WaitForSeconds(1);
        currentBullet = bulletMax;
        isReloading = false;
    }

    public void PlayMagOutSound()
    {
        AudioSource.PlayClipAtPoint(clipReload, Camera.main.transform.position);
    }
    public void SpawnMuzzle()
    {
        if (MuzzlePrefab != null && PointFx != null)
        {
            var muzzle = Instantiate(MuzzlePrefab, PointFx.position, Quaternion.Euler(new Vector3(180, 0, 180)), transform.parent);
            Destroy(muzzle, 0.3f);
        }
    }

    public void SpawnSmoke()
    {
        if (MuzzlePrefab != null && PointFx != null)
        {
            MuzzleParticleSystem.transform.localPosition = Vector3.zero;
            MuzzleParticleSystem.transform.localRotation = Quaternion.identity;
            var ps = MuzzleParticleSystem.GetComponent<ParticleSystem>();
            ps.Play();
        }
    }

    private IEnumerator DelayStopSmoke()
    {
        while (isFiring) yield return null;

        if (!isSmoking)
        {
            isSmoking = true;
            SpawnSmoke();
            yield return new WaitForSeconds(MuzzleParticleSystem.main.duration);
            isSmoking = false;
            MuzzleParticleSystem.Stop();
        }
    }

    public void ToggleAndroidFlashlight()
    {
        if (camera == null)
        {
            var cameraClass = new AndroidJavaClass("android.hardware.Camera");
            camera = cameraClass.CallStatic<AndroidJavaObject>("open", 0);
            if (camera != null)
            {
                cameraParameters = camera.Call<AndroidJavaObject>("getParameters");
                cameraParameters.Call("setFlashMode", "torch");
                camera.Call("setParameters", cameraParameters);
            }
        }
        else
        {
            cameraParameters = camera.Call<AndroidJavaObject>("getParameters");
            var flashmode = cameraParameters.Call<string>("getFlashMode");
            if (flashmode != "torch")
                cameraParameters.Call("setFlashMode", "torch");
            else
                cameraParameters.Call("setFlashMode", "off");

            camera.Call("setParameters", cameraParameters);
        }
    }

    private void ReleaseAndroidJavaObjects()
    {
        if (camera != null)
        {
            camera.Call("release");
            camera = null;
        }
    }
    
    private IEnumerator DisableBullet(GameObject bullet)
    {
        yield return new WaitForSeconds(2.0f);
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vape : MonoBehaviour
{
    [SerializeField] private float juiceMax;
    [SerializeField] private float juiceFireSpeed;
    [SerializeField] private float juiceCurrent;
    [SerializeField] public float vapeScaleTime;
    private bool isFillingUp;
    [SerializeField] public Image lungBar;
    [SerializeField] public Collider2D vape;
    private bool isSucking = true;
    public ParticleSystem Smoke;
    private readonly float suckDuration = 2f;
    private float suckTimer;
    private bool isTouchingScreen = true;
    private bool shouldSpawnSmoke;

    [SerializeField] private ParticleSystem[] particleEffects;
    [SerializeField] private float[] effectTimes;
    [SerializeField] private float minimumTime;
    [SerializeField] private float maximumTime = 10f;
    private int index = -1;

    public AudioClip vapeStart;
    public AudioClip vapeOff;
    public AudioClip vapeLoop;

    private AndroidJavaObject camera;
    private AndroidJavaObject cameraParameters;
    private void Update()
    {
        UpdateSuckState();
    }

    private void UpdateSuckState()
    {
        if (Input.GetMouseButton(0) && lungBar.fillAmount > 0)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (vape == Physics2D.OverlapPoint(ray.origin))
            {
                IsSuckingCoroutine();
                ToggleAndroidFlashlight();
                suckTimer += Time.deltaTime;
                var fillPercent = suckTimer / suckDuration;
                lungBar.fillAmount = 1 - fillPercent;
                var timePercentage = fillPercent * (maximumTime - minimumTime) + minimumTime;
                for (var i = 0; i < effectTimes.Length; i++)
                    if (timePercentage <= effectTimes[i] || i == effectTimes.Length - 1)
                    {
                        index = i;
                        break;
                    }

                if (lungBar.fillAmount == 0) resetLungBar();
                if (lungBar.fillAmount == 1) suckTimer = 0f;
                AudioSource.PlayClipAtPoint(vapeStart, Camera.main.transform.position);
            }
            else
            {
                EndSuck();
                isSucking = false;
                index = -1;
            }

            isTouchingScreen = true;
        }

        if (Input.GetMouseButtonUp(0) && lungBar.fillAmount > 0 && isTouchingScreen)
        {
            isTouchingScreen = false;
            shouldSpawnSmoke = true; // Khi nhả chuột ra, set biến shouldSpawnSmoke thành true
            // Hiển thị particle effect tương ứng nếu thỏa mãn điều kiện và đang hút
            if (index > 0 && index < particleEffects.Length && isSucking) SpawnParticleEffect(particleEffects[index]);
            AudioSource.PlayClipAtPoint(vapeOff, Camera.main.transform.position);
            index = -1;
        }
    }

    private void IsSuckingCoroutine()
    {
        {
            if (!isSucking)
            {
                StartCoroutine(SuckCoroutine());
                isSucking = true;
            }
        }
    }

    private IEnumerator SuckCoroutine()
    {
        while (Input.GetMouseButton(0) && lungBar.fillAmount > 0)
        {
            juiceCurrent -= juiceFireSpeed * Time.deltaTime;
            juiceCurrent = Mathf.Clamp(juiceCurrent, 0, juiceMax);

            // Update juice mesh để hiển thị giá trị mới
            var juiceMesh = GetComponentInChildren<MeshFilter>().mesh;
            var submeshIndex = Mathf.RoundToInt((1 - juiceCurrent / juiceMax) * (juiceMesh.subMeshCount - 1));
            yield return null;
        }
        
        isSucking = false;
    }
    

    public void FillJuice(float amount)
    {
        juiceCurrent += amount;
        juiceCurrent = Mathf.Clamp(juiceCurrent, 0, juiceMax);

        // Update juice mesh để hiển thị giá trị mới
        var juiceMesh = GetComponentInChildren<MeshFilter>().mesh;
        var submeshIndex = Mathf.RoundToInt((1 - juiceCurrent / juiceMax) * (juiceMesh.subMeshCount - 1));
        juiceMesh.subMeshCount = submeshIndex;
    }

    private IEnumerator ReleaseFlashlight(float time)
    {
        yield return new WaitForSeconds(time);
        ReleaseAndroidJavaObjects();
    }

    private void releaseAndroidJavaObjects()
    {
        if (camera != null)
        {
            camera.Call("release");
            camera = null;
        }
    }

    private void SpawnParticleEffect(ParticleSystem particleEffect)
    {
        particleEffect.transform.localPosition = Vector3.zero;
        particleEffect.transform.localRotation = Quaternion.identity;
        var ps = particleEffect.GetComponent<ParticleSystem>();
        ps.Play();
    }

    private void resetLungBar()
    {
        lungBar.fillAmount = 1;
    }

    private void SpawnSmoke()
    {
        Smoke.transform.localPosition = Vector3.zero;
        Smoke.transform.localRotation = Quaternion.identity;
        var ps = Smoke.GetComponent<ParticleSystem>();
        ps.Play();
    }

    private void ToggleAndroidFlashlight()
    {
        try
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
        catch (Exception e)
        {
            Debug.Log($"Failed to toggle flashlight: {e}");
        }
    }

    private void EndSuck()
    {
        if (!isSucking) return;
        isFillingUp = true;
        AudioSource.PlayClipAtPoint(vapeOff, Camera.main.transform.position);
    }


    private void ReleaseAndroidJavaObjects()
    {
        if (camera != null)
        {
            camera.Call("release");
            camera = null;
        }
    }
}



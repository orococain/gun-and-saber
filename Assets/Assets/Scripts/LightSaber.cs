using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class LightSaber : MonoBehaviour
{
    public List<Material> glowMaterial;
    public List<Material> saberGlowMaterial;
    public List<ParticleSystem> electicFx;
    public List<ParticleSystem> effectColor;
    public Color saberColor;
    public float intensityGlow;
    public float intensitySaber;
    public GameObject saberPrefab;
    public List<Transform> lightSaber;
    private bool isHolding = false;
    public Image powerBar;
    public float maxPower = 100f;
    public float currentPower;
    public bool isPowered;
    public BoxCollider2D saberCollider;

    public Slider colorSlider;

    // public GameObject reloadPanel; // UI Canvas để hiển thị thông báo pop-up
    public float reloadTime = 2f;
    private bool isReadyToPlay = true;
    public float powerConsumptionHold = 10f;
    private float holdTime = 0f;
    public float timeToReducePower = 0.25f;
    public AudioSource audioSource;
    public AudioClip saberOnSound;
    public AudioClip saberoff;
    AndroidJavaObject camera=null;
    AndroidJavaObject cameraParameters=null;
    private bool isSaberOn = false;
    public void Start()
    {
        currentPower = maxPower;
        powerBar.fillAmount = 1;
        colorSlider.onValueChanged.AddListener(delegate { ChangeSaberColor(colorSlider.value); });
    }

    public void Update()
    {
        IsInputing();
        if (currentPower <= 0 )
        {
            // Nếu năng lượng đã hết, tắt lưỡi kiếm và effect
            foreach (Transform saber in lightSaber)
            {
                if (saber.GetChild(0).gameObject.activeSelf)
                {
                    saber.GetChild(0).gameObject.SetActive(false);
                    powerBar.fillAmount = currentPower / maxPower;
                    SetGlow(false, saber.GetChild(0).transform);
                    AudioSource.PlayClipAtPoint(saberoff, Camera.main.transform.position);
                    foreach (ParticleSystem effect in electicFx)
                    {
                        effect.Stop();
                    }
                    ReleaseAndroidJavaObjects();
                }
            }

            isHolding = false;
            holdTime = 0f;
            StartCoroutine(ResetReload());
        }
    }

    public void IsInputing()

    {
        if (Input.GetMouseButton(0) && !isHolding && isPowered)
        {
            // Kiểm tra năng lượng còn đủ để bật lưỡi kiếm hay không
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (saberCollider == Physics2D.OverlapPoint(ray.origin))
            {
                if (currentPower > 0)
                {
                    // Bật lưỡi kiếm và giảm năng lượng
                    foreach (Transform saber in lightSaber)
                    {
                        if (!saber.GetChild(0).gameObject.activeSelf)
                        {
                            saber.GetChild(0).gameObject.SetActive(true);
                            SetGlow(true, saber.GetChild(0).transform);
                            currentPower -= 20f * Time.deltaTime;
                            powerBar.fillAmount = currentPower / maxPower;
                            foreach (ParticleSystem effect in electicFx)
                            {
                                effect.Play();
                            }
                            ToggleAndroidFlashlight();
                            AudioSource.PlayClipAtPoint(saberOnSound, Camera.main.transform.position);
                        }
                    }

                    isHolding = true;
                    holdTime = 0f;
                }
                
            }
        }
        else if (Input.GetMouseButton(0) && isHolding && isPowered)
        {
            holdTime += Time.deltaTime;

            if (holdTime >= timeToReducePower)
            {
                // Giảm năng lượng khi giữ chuột trái
                currentPower -= powerConsumptionHold;
                powerBar.fillAmount = currentPower / maxPower;
                holdTime = 0f;
            }
        
        }
        else if (!Input.GetMouseButton(0) && isHolding  )
        {
            // Tắt lưỡi kiếm và tắt particle effect
            foreach (Transform saber in lightSaber)
            {

                if (saber.GetChild(0).gameObject.activeSelf)
                {
                    saber.GetChild(0).gameObject.SetActive(false);
                    SetGlow(false, saber.GetChild(0).transform);
                    powerBar.fillAmount = currentPower / maxPower;
                    foreach (ParticleSystem effect in electicFx)
                    {
                        effect.Stop();
                    }
                    ReleaseAndroidJavaObjects();
                }
            }

            isHolding = false;
            holdTime = 0f;
        }
    }

    IEnumerator ResetReload()
    {
        yield return new WaitForSeconds(reloadTime);

        // Tái nạp năng lượng
        Reload();
        powerBar.gameObject.SetActive(true);
        isReadyToPlay = true;
    }

    public void Reload()
    {
        currentPower = maxPower;
        powerBar.fillAmount = 1;
        isPowered = true;
    }

    void SetGlow(bool glow, Transform saber)
    {
        Color albedoColor = Color.blue;

        if (glow)
        {
            albedoColor = saberColor * intensityGlow;
        }

        foreach (Transform child in saber)
        {
            MeshRenderer meshRenderer = child.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                Material[] materials = meshRenderer.materials;
                foreach (Material material in materials)
                {
                    material.SetColor("_Color", albedoColor);
                    material.EnableKeyword("_COlor");
                }
                meshRenderer.materials = materials;
            }
        }
        Debug.Log("glow");
    }


    public void ChangeSaberColor(float value)
    {
        saberColor = Color.HSVToRGB(value, 1f, 1f);

        foreach (Material material in glowMaterial)
        {
            material.SetColor("Color", saberColor);
        }

        foreach (Material material in saberGlowMaterial)
        {
            material.SetColor("Color", saberColor);
        }

        // Kiểm tra xem màu đã thay đổi hay chưa
        Debug.Log("Saber Color: " + saberColor);
    }
    
    public  void ToggleAndroidFlashlight()
    {

        if (camera == null)
        {
            AndroidJavaClass cameraClass = new AndroidJavaClass("android.hardware.Camera"); 
            camera = cameraClass.CallStatic<AndroidJavaObject>("open", 0); 
            if (camera != null)
            {
                cameraParameters = camera.Call<AndroidJavaObject>("getParameters");
                cameraParameters.Call("setFlashMode","torch"); 
                camera.Call("setParameters",cameraParameters); 
            }       
        }
        else
        {
            cameraParameters = camera.Call<AndroidJavaObject>("getParameters");
            string flashmode = cameraParameters.Call<string>("getFlashMode");
            if(flashmode!="torch")
                cameraParameters.Call("setFlashMode","torch"); 
            else
                cameraParameters.Call("setFlashMode","off"); 

            camera.Call("setParameters",cameraParameters); 
        }
    }

    void ReleaseAndroidJavaObjects()
    {
        if (camera != null)
        {
            camera.Call("release");
            camera = null;
        }
    }
}


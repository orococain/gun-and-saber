using System;
using System.Collections;
using System.Collections.Generic;
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
    public void Start()
    {
        currentPower = maxPower;
        powerBar.fillAmount = 1;
        colorSlider.onValueChanged.AddListener(ChangeSaberColor);
    }

    public void Update()
    {
        IsInputing();
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
                else if (isReadyToPlay) // Năng lượng đã hết và người chơi sẵn sàng thay đạn
                {
                    // Hiển thị thông báo pop-up và chờ trong một khoảng thời gian nhất định
                    // reloadPanel.SetActive(true);
                    isReadyToPlay = false;
                    StartCoroutine(ResetReload());
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
        else if (!Input.GetMouseButton(0) && isHolding)
        {
            // Tắt lưỡi kiếm và tắt particle effect
            foreach (Transform saber in lightSaber)
            {

                if (saber.GetChild(0).gameObject.activeSelf)
                {
                    saber.GetChild(0).gameObject.SetActive(false);
                    SetGlow(false, saber.GetChild(0).transform);
                    powerBar.fillAmount = currentPower / maxPower;
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
        }
    }

    IEnumerator ResetReload()
    {
        yield return new WaitForSeconds(reloadTime);

        // Tái nạp năng lượng
        Reload();
        powerBar.gameObject.SetActive(true);
        //reloadPanel.SetActive(false);
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
        foreach (Material material in glowMaterial)
        {
            material.SetColor("_EmissionColor", saberColor * intensityGlow);
            // material.SetFloat("_EmissionIntensity", Mathf.Lerp(material.GetFloat("_EmissionIntensity"), glow ? 1 : 0, 0.15f));
        }

        foreach (Material material in saberGlowMaterial)
        {
            material.SetColor("_EmissionColor", saberColor * intensitySaber);
            //material.SetFloat("_EmissionIntensity", Mathf.Lerp(material.GetFloat("_EmissionIntensity"), glow ? 1 : 0, 2f));
        }
    }

    public void ChangeSaberColor(float value)
    {
        // Lấy giá trị màu từ slider
        saberColor = Color.HSVToRGB(value, 1f, 1f);

        // Cập nhật màu lưỡi kiếm
        foreach (Transform saber in lightSaber)
        {
            MeshRenderer saberRenderer = saber.GetComponentInChildren<MeshRenderer>();

            // Cập nhật màu sáng của vật liệu Glow
            foreach (Material material in glowMaterial)
            {
                material.SetColor("_EmissionColor", saberColor * intensityGlow);
            }

            // Cập nhật màu sáng của vật liệu Saber Glow
            foreach (Material material in saberGlowMaterial)
            {
                material.SetColor("_EmissionColor", saberColor * intensitySaber);
            }

            // Cập nhật màu sắc của texture và vật liệu của lưỡi kiếm
            if (saberRenderer != null && saberRenderer.materials != null)
            {
                foreach (Material material in saberRenderer.materials)
                {
                    if (saberGlowMaterial.Contains(material))
                    {
                        Texture2D mainTexture = (Texture2D)material.mainTexture;
                        Texture2D newTexture = new Texture2D(mainTexture.width, mainTexture.height);
                        Color[] pixels = mainTexture.GetPixels();
                        for (int i = 0; i < pixels.Length; i++)
                        {
                            pixels[i] *= saberColor;
                        }

                        newTexture.SetPixels(pixels);
                        newTexture.Apply();
                        material.mainTexture = newTexture;
                    }
                }
            }

        }
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


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

    public void Start()
    {
        currentPower = maxPower;
        powerBar.fillAmount = 1;
       // reloadPanel.SetActive(false);
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
                    }
                }

                isHolding = true;
                currentPower--;
            }
            else if (isReadyToPlay) // Năng lượng đã hết và người chơi sẵn sàng thay đạn
            {
                // Hiển thị thông báo pop-up và chờ trong một khoảng thời gian nhất định
               // reloadPanel.SetActive(true);
                isReadyToPlay = false;
                StartCoroutine(ResetReload());
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
                        foreach (ParticleSystem effect in electicFx)
                        {
                            effect.Stop();
                        }
                    }
                }
                isHolding = false;

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
            // material.SetFloat("_EmissionIntensity",
            //     Mathf.Lerp(material.GetFloat("_EmissionIntensity"), glow ? 1 : 0, 0.15f));
        }

        foreach (Material material in saberGlowMaterial)
        {
            material.SetColor("_EmissionColor", saberColor * intensitySaber);
            // material.SetFloat("_EmissionIntensity",
            //     Mathf.Lerp(material.GetFloat("_EmissionIntensity"), glow ? 1 : 0, 0.15f));
        }
    }
    
    public void ChangeSaberColor(float value)
    {
        // Lấy giá trị màu từ slider
        saberColor = Color.HSVToRGB(value, 1f, 1f);

        // Cập nhật màu lưỡi kiếm
        foreach (Material material in glowMaterial)
        {
            material.SetColor("_EmissionColor", saberColor * intensityGlow);
        }

        foreach (Material material in saberGlowMaterial)
        {
            material.SetColor("_EmissionColor", saberColor * intensitySaber);
        }
    }
}


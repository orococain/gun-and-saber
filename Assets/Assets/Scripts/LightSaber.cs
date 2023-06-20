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
    public Image powerBar; // Thanh Image hiển thị cho năng lượng
    public float maxPower = 100f; // Giá trị tối đa của năng lượng
    public float currentPower; // Giá trị hiện tại của năng lượng
    public bool isPowered; // Trạng thái của năng lượng - đã nạp hoặc chưa
    public BoxCollider2D saberCollider;
    public void Start()
    {
        currentPower = maxPower;
        powerBar.fillAmount = 1;
    }


    public void Update()
    {
        IsInputing();
    }

    public void IsInputing()
    {
        {
            // Kiểm tra nếu đang giữ màn hình thì xuất hiện lưỡi kiếm và tăng giảm thanh năng lượng
            if (Input.GetMouseButton(0) && !isHolding && isPowered)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (saberCollider == Physics2D.OverlapPoint(ray.origin))
                {
                    foreach (Transform saber in lightSaber)
                    {
                        // Tăng giảm thanh năng lượng tùy thuộc vào việc bật hay tắt lưỡi kiếm
                            if (!saber.GetChild(0).gameObject.activeSelf)
                            {
                                if (currentPower > 0)
                                {
                                    saber.GetChild(0).gameObject.SetActive(true);
                                    SetGlow(true, saber.GetChild(0).transform);
                                    currentPower -= 10f * Time.deltaTime;
                                    powerBar.fillAmount = currentPower / maxPower;
                                    foreach (ParticleSystem effect in electicFx)
                                    {
                                        effect.Play();
                                    }
                                }
                                else
                                {
                                    isPowered = false;
                                    powerBar.gameObject.SetActive(false);
                                }
                            }

                            isHolding = true;
                    }
                }
            }
            // Kiểm tra nếu không giữ màn hình thì ẩn lưỡi kiếm đi và tắt particle effect
            else if (!Input.GetMouseButton(0) && isHolding)
            {
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
}

public void Reload()
{
    // Nạp lại năng lượng
    currentPower = maxPower;
    powerBar.fillAmount = 1;
    powerBar.gameObject.SetActive(true);
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
}


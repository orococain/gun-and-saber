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
    public float powerConsumptionRate = 10f; // Số đơn vị năng lượng sẽ bị tiêu thụ mỗi giây (khi khởi động)
    public float powerConsumptionHoldRate = 20f; // Số đơn vị năng lượng sẽ bị tiêu thụ mỗi giây (khi giữ nút)

    private float currentConsumptionRate; // Số đơn vị năng lượng sẽ bị tiêu thụ mỗi giây

    public void Start()
    {
        currentPower = maxPower;
        powerBar.fillAmount = 1;
        currentConsumptionRate = powerConsumptionRate; // khởi động với lượng tiêu thụ mặc định
    }


    public void Update()
    {
        IsInputing();
    }

    public void IsInputing()
    {
      if (Input.GetMouseButton(0) && !isHolding && isPowered)
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

                    // Sử dụng tỷ lệ tiêu thụ năng lượng khác nhau tùy thuộc vào việc giữ hay không giữ nút nhấn
                    if (Input.GetMouseButton(0))
                    {
                        currentPower -= currentConsumptionRate * Time.deltaTime;
                    }
                    else
                    {
                        currentPower -= currentConsumptionRate * Time.deltaTime;
                    }

                    // Kiểm tra xem lượng năng lượng còn lại có đủ để bật lưỡi kiếm
                    if (currentPower < 0)
                    {
                        currentPower = 0;
                        isPowered = false;
                        powerBar.gameObject.SetActive(false);
                    }
                    else
                    {
                        powerBar.fillAmount = currentPower / maxPower;
                    }
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
        }

        isHolding = true;
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
        currentConsumptionRate = powerConsumptionRate; // Trở về lượng tiêu thụ mặc định khi người chơi thả nút
    }
}

public void Reload()
{
    // Nạp lại năng lượng
    currentPower = maxPower;
    powerBar.fillAmount = 1;
    powerBar.gameObject.SetActive(true);
    isPowered = true;
    currentConsumptionRate = powerConsumptionRate; // Trở về lượng tiêu thụ mặc định khi người chơi nạp lại năng lượng
}
    public void EndInput()
    {
    }

    public void StartInput3D()
    {
    }

    public void ChangeColorEditor()
    {
    }

    public void OnChangeColor(Color color)
    {
    }

    public void ScaleSaber(bool isOn)
    {
    }

    public void ResetSaber()
    {
    }

    public void PlayFx(bool state)
    {
    }

    void SetGlow(bool glow, Transform saber)
    {
        foreach (Material material in glowMaterial)
        {
            material.SetColor("_EmissionColor", saberColor * intensityGlow);
            material.SetFloat("_EmissionIntensity",
                Mathf.Lerp(material.GetFloat("_EmissionIntensity"), glow ? 1 : 0, 0.15f));
        }

        foreach (Material material in saberGlowMaterial)
        {
            material.SetColor("_EmissionColor", saberColor * intensitySaber);
            material.SetFloat("_EmissionIntensity",
                Mathf.Lerp(material.GetFloat("_EmissionIntensity"), glow ? 1 : 0, 0.15f));
        }
    }
}


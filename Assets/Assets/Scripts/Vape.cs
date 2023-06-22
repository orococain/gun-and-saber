using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vape : MonoBehaviour
{
    [SerializeField]
    private float juiceMax; // hàm lượng juice tối đa
    
    [SerializeField]
    private float juiceFireSpeed; // tốc độ tiêu hao juice

    private float juiceCurrent; // hàm lượng juice hiện tại

    public float vapeScaleTime;

    private bool isFillingUp;

    public RectTransform container;

    public float vapeScaleMin;

    public float vapeScaleMax;

    public Image lungBar; // thanh hiển thị lượng hơi thở còn lại

    public Collider2D vape; // Collider của vape

    private bool isSucking = true; // Biến kiểm tra có đang giữ chạm màn hình hay không

    public ParticleSystem Smoke; // hiệu ứng hơi thở

    private float suckDuration = 2f; // thời gian hút tối đa (giây)
    private float suckTimer; 
    private bool isTouchingScreen = true;
    bool shouldSpawnSmoke = false;// đồng hồ đếm thời gian hút
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (vape == Physics2D.OverlapPoint(ray.origin))
            {
                if (!isSucking && juiceCurrent > 0)
                {
                    isSucking = true;
                    suckTimer = 0f;
                    isTouchingScreen = true;
                    StartSuck();
                }
            }
        }

        if (isSucking)
        {
            if (Input.GetMouseButton(0) && suckTimer < suckDuration)
            {
                IsSucking();
                suckTimer += Time.deltaTime;
                float fillPercent = suckTimer / suckDuration;
                lungBar.fillAmount = 1 - fillPercent;
            }
            else 
            {
                EndSuck();
                isSucking = false;
                lungBar.fillAmount = 1;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isTouchingScreen = false;
            shouldSpawnSmoke = true; // Khi nhả chuột ra, set biến shouldSpawnSmoke thành true
        }
    }

    public void Start()
    {
        juiceCurrent = juiceMax;
        KeepJuiceColor();
    }
    
    void UpdateJuice()
    {
        float juicePercentage = juiceCurrent / juiceMax;
        Image juiceColor = container.GetComponentInChildren<Image>();
        juiceColor.color = Color.Lerp(Color.red, Color.green, juicePercentage);
    }

    public void StartSuck()
    {
        juiceCurrent -= juiceFireSpeed * Time.deltaTime;
        juiceCurrent = Mathf.Clamp(juiceCurrent, 0, juiceMax);
        UpdateJuice();
        isTouchingScreen = true;
    }

    public void IsSucking()
    {
        juiceCurrent -= juiceFireSpeed * Time.deltaTime;
        juiceCurrent = Mathf.Clamp(juiceCurrent, 0, juiceMax);
        container.localScale = new Vector3(Mathf.Lerp(vapeScaleMin, vapeScaleMax, juiceCurrent / juiceMax), container.localScale.y, container.localScale.z);
      
    }

    public void EndSuck()
    {
        SpawnSmoke();
            isFillingUp = true;
        StartCoroutine(FillUp(vapeScaleTime));
    }

    public void FillUpJuice(bool isBoostEnergy = false)
    {
        juiceCurrent = juiceMax * 0.75f;
        UpdateJuice();
    }

    protected IEnumerator FillUp(float time)
    {
        yield return new WaitForSeconds(time);
        juiceCurrent = Mathf.Clamp(juiceCurrent, 0, juiceMax);
        
        KeepJuiceColor();
    }
    

    protected void KeepJuiceColor()
    {
    }
    
    
    public void SpawnSmoke()
    {
        Smoke.transform.localPosition = Vector3.zero;
        Smoke.transform.localRotation = Quaternion.identity;
        ParticleSystem ps = Smoke.GetComponent<ParticleSystem>();
        ps.Play();
    }
}



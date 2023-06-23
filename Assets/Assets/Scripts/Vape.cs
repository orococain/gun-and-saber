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
    [SerializeField] private ParticleSystem[] particleEffects; // Mảng các particle effect để hiển thị
    [SerializeField] private float[] effectTimes; // Thời gian bấm giữ tương ứng với mỗi particle effect
    [SerializeField] private float minimumTime = 0f; // Thời gian bấm giữ tối thiểu để hiển thị particle effect
    [SerializeField] private float maximumTime = 10f; // Thời gian bấm giữ tối đa để hiển thị particle effect
    private int index = -1; // Chỉ số particle effect đang được hiển thị

    public void Update()
    {
        if (Input.GetMouseButton(0) && lungBar.fillAmount > 0)
        {
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (vape == Physics2D.OverlapPoint(ray.origin))
            {
                IsSucking();
                suckTimer += Time.deltaTime;
                float fillPercent = suckTimer / suckDuration;
                lungBar.fillAmount = 1 - fillPercent;
                // Tìm chỉ số particle effect tương ứng dựa trên thời gian bấm giữ
                float timePercentage = fillPercent * (maximumTime - minimumTime) + minimumTime;
                for (int i = 0; i < effectTimes.Length; i++)
                {
                    if (timePercentage <= effectTimes[i] || i == effectTimes.Length - 1)
                    {
                        index = i;
                        break;
                    }
                }

                if (lungBar.fillAmount == 0)
                {
                    resetLungBar();
                }
                Debug.Log("Smoke");
                if (lungBar.fillAmount == 1)
                {
                    suckTimer = 0f; // Reset suckTimer khi lung bar đã đầy
                }
            }
            else
            {
                EndSuck();
                isSucking = false;
                index = -1;
            }
            isTouchingScreen = true;
        
            if (shouldSpawnSmoke && !Smoke.isPlaying)
            {
                SpawnSmoke();
                shouldSpawnSmoke = false;
            }
        }
        if (Input.GetMouseButtonUp(0) && lungBar.fillAmount > 0 && isTouchingScreen) 
        {
            isTouchingScreen = false;
            shouldSpawnSmoke = true; // Khi nhả chuột ra, set biến shouldSpawnSmoke thành true
            // Hiển thị particle effect tương ứng nếu thỏa mãn điều kiện và đang hút
            if (index >= 0 && index < particleEffects.Length && isSucking)
            {
                SpawnParticleEffect(particleEffects[index]);
            }

            index = -1; 
        }
    }
    

    public void resetLungBar()
    {
        lungBar.fillAmount = 1;
        isTouchingScreen = true; 
    }

    public void Start()
    {
        juiceCurrent = juiceMax;
        KeepJuiceColor();
    }
    
    private void SpawnParticleEffect(ParticleSystem particleEffect)
    {
        // Hiển thị particle effect
        particleEffect.transform.localPosition = Vector3.zero;
        particleEffect.transform.localRotation = Quaternion.identity;
        ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();
        ps.Play();
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
        if (!isSucking) return;
        SpawnSmoke();
        isFillingUp = true;
        StartCoroutine(FillUp(vapeScaleTime));
        isSucking = false;
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



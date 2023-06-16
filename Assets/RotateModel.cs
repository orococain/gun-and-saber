using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class RotateModel : MonoBehaviour
{
    public GameObject uiCanvas;
    public Collider2D gunCollider;
    public float rotationSpeed;

    private bool isUIVisible = true;

    void Start()
    {
        ShowUI();
    }

    void Update()
    {
        if (!isUIVisible)
        {
            // Xoay model bằng các phím mũi tên
            float rotationY = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationY, 0);
        }
    }

    public void OnButtonClicked()
    {
        HideUI();
        gunCollider.enabled = true;
    }

    private void ShowUI()
    {
        uiCanvas.SetActive(true);
        isUIVisible = true;
    }

    private void HideUI()
    {
        uiCanvas.SetActive(false);
        isUIVisible = false;
    }

}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomIN : MonoBehaviour
{
    public Camera zoomCamera; // Camera orthographic để zoom
    public Canvas zoomCanvas; // Canvas chứa các UI được zoom

    private Vector3 initialCameraPosition; // Vị trí ban đầu của camera
    private float initialCameraSize; // Kích thước ban đầu của camera
    private Vector2 initialCanvasScale; // Tỉ lệ ban đầu của canvas

    private bool zoomedIn; // Biến kiểm tra xem camera có đang được zoom hay không

    public void ZoomIn()
    {
        // Lưu trữ vị trí ban đầu trên màn hình và chuyển đổi nó thành một vector trong không gian thế giới
        Vector3 worldPosition = zoomCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = zoomCamera.transform.position.z;

        // Di chuyển camera đến vị trí bấm và zoom vào
        zoomCamera.transform.position = worldPosition;
        zoomCamera.orthographicSize /= 2f;
        zoomCanvas.transform.localScale *= 2f;
        zoomedIn = true;
    }

    public void ZoomOut()
    {
        // Khôi phục giá trị ban đầu của camera và canvas
        zoomCamera.transform.position = initialCameraPosition;
        zoomCamera.orthographicSize = initialCameraSize;
        zoomCanvas.transform.localScale = initialCanvasScale;
        zoomedIn = false;
    }

    private void Start()
    {
        // Lưu trữ các giá trị ban đầu của camera và canvas
        initialCameraPosition = zoomCamera.transform.position;
        initialCameraSize = zoomCamera.orthographicSize;
        initialCanvasScale = zoomCanvas.transform.localScale;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZoomIN : MonoBehaviour
{
    public Camera zoomCamera; // Camera orthographic để zoom
    public GameObject uiElement1; // Canvas chứa các UI được zoom
    public GameObject uiElement2;
    public GameObject targetObject; // Object mà muốn zoom tới

    private Vector3 initialCameraPosition; // Vị trí ban đầu của camera
    private float initialCameraSize; // Kích thước ban đầu của camera
    private Vector2 initialCanvasScale; // Tỉ lệ ban đầu của canvasem camera có đang được zoom hay không
    private bool zoomedIn;
    public void ZoomIn()
    {
        // Nếu đang zoom thì thực hiện hành động zoom out, ngược lại thực hiện hành động zoom in
        if (zoomedIn)
        {
            // Khôi phục giá trị ban đầu của camera và canvas
            zoomCamera.transform.position = initialCameraPosition;
            zoomCamera.orthographicSize = initialCameraSize;
            zoomedIn = false;
            // Bật lại canvas khi zoom out
            uiElement1.gameObject.SetActive(true);
            uiElement2.gameObject.SetActive(true);
        }
        else
        {
            Vector3 worldPosition = zoomCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = zoomCamera.transform.position.z;

            // Di chuyển camera đến vị trí bấm và zoom vào
            zoomCamera.transform.position = worldPosition;
            zoomCamera.orthographicSize /= 2f;

            // Ẩn hai phần tử uiElement1 và uiElement2 trong canvas
            uiElement1.gameObject.SetActive(false);
            uiElement2.gameObject.SetActive(false);

            zoomedIn = true;
        }
    }

    private void Start()
    {
        // Lưu trữ các giá trị ban đầu của camera và canvas
        initialCameraPosition = zoomCamera.transform.position;
        initialCameraSize = zoomCamera.orthographicSize;
    
    }
}

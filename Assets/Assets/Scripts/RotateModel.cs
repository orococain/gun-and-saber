using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class RotateModel : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public List<Transform> itemTransformList = new List<Transform>();
    public BoxCollider2D shootCollider;
    public GameObject UILeft;
    public GameObject UIRight;
    
    private bool isActive = false;
    public float rotateSpeed = 0.1f;
    
    public void ToggleActive()
    {
        isActive = !isActive;
        shootCollider.enabled = !isActive; // toggle the collider

        if (isActive)
        {
            // if not active, hide the rotation UI and reset the rotations of the items
            UIRight.SetActive(false);
            UILeft.SetActive(false);
            
        }
        else
        {
            // if active, show the rotation UI
            UIRight.SetActive(true);
            UILeft.SetActive(true);
            foreach (Transform transform in itemTransformList)
            {
                transform.eulerAngles = Vector3.zero;
            }
        }
        
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
    
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isActive)
        {
            foreach (Transform transform in itemTransformList)
            {
                transform.eulerAngles += new Vector3(-eventData.delta.y * rotateSpeed, eventData.delta.x * rotateSpeed, 0);
            }
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemTransformList.Contains(eventData.pointerDrag.transform))
        {
            itemTransformList.Remove(eventData.pointerDrag.transform);
        }
    }
}




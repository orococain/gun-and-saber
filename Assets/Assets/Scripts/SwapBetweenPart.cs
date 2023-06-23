using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapBetweenPart : MonoBehaviour
{
    public GameObject[] headParts;
    public GameObject[] batteryParts;

    private int currentHeadIndex = 0;
    private int currentBatteryIndex = 0;

    private void Start()
    {
        // Set initial head and battery parts
        SetHeadPart(0);
        SetBatteryPart(0);
    }

    public void SetHeadPart(int index)
    {
        // Disable current head part
        headParts[currentHeadIndex].SetActive(false);

        // Enable new head part
        headParts[index].SetActive(true);

        // Update current head index
        currentHeadIndex = index;
    }

    public void SetBatteryPart(int index)
    {
        // Disable current battery part
        batteryParts[currentBatteryIndex].SetActive(false);

        // Enable new battery part
        batteryParts[index].SetActive(true);

        // Update current battery index
        currentBatteryIndex = index;
    }
}

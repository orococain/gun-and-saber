using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vapepart : MonoBehaviour
{
    
    [SerializeField] private GameObject[] vapeParts;
    private int currentVapePartIndex = 0;
    // Start is called before the first frame update
    public void SetType(int index)
    {
        if (index >= 0 && index < vapeParts.Length)
        {
            // Disable current vape part
            vapeParts[currentVapePartIndex].SetActive(false);
            currentVapePartIndex = index;
            // Enable new vape part
            vapeParts[currentVapePartIndex].SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapBetweenPart : MonoBehaviour
{
    public GameObject[] tankHeads;
    public GameObject[] tankBodies;
    public int headIndex = 0;
    public int bodyIndex = 0;

    void Start(){
        // set default head and body
        tankHeads[headIndex].SetActive(true);
        tankBodies[bodyIndex].SetActive(true);
    }

    public void ChangeHead(){
        // disable current head
        tankHeads[headIndex].SetActive(false);

        // change head index to the next or previous one
        headIndex = (headIndex + 1) % tankHeads.Length;

        // enable new head
        tankHeads[headIndex].SetActive(true);
    }

    public void ChangeBody(){
        // disable current body
        tankBodies[bodyIndex].SetActive(false);

        // change body index to the next or previous one
        bodyIndex = (bodyIndex + 1) % tankBodies.Length;

        // enable new body
        tankBodies[bodyIndex].SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class AtlasUser : MonoBehaviour
{
    [SerializeField]  SpriteAtlas atLas;

    [SerializeField] private string spriteName;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = atLas.GetSprite(spriteName);
    }

  
}

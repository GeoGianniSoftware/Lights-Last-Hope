using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideUIOnStart : MonoBehaviour
{
    Image imageToHide;
    // Start is called before the first frame update
    void Start()
    {
        imageToHide = GetComponent<Image>();
        imageToHide.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthUIHeart : MonoBehaviour
{
    public Sprite fullSprite, emptySprite;
    public bool full;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (full && GetComponent<Image>().sprite == emptySprite) {
            GetComponent<Image>().sprite = fullSprite;
        }
        else if(!full && GetComponent<Image>().sprite == fullSprite) {
            GetComponent<Image>().sprite = emptySprite;
        }
    }
}

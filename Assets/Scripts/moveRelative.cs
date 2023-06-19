using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class moveRelative : MonoBehaviour
{
    public RectTransform relativeObject;
    public Vector3 offset;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = relativeObject.transform.position + (Vector3.down * relativeObject.rect.height) + offset;
    }
}

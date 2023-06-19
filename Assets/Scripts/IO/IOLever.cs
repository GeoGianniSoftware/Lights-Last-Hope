using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOLever : IOInput
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Animator>()) {
            GetComponent<Animator>().SetBool("Activated", activated);
        }
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractText))]
[RequireComponent(typeof(Rigidbody))]
public class PickupObject : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Interact() {
        FindObjectOfType<InteractionController>().GrabObject(this);
    }
}

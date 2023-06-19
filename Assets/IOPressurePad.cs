using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOPressurePad : IOInput
{
    InteractionController interact;
    // Start is called before the first frame update
    void Start()
    {
        interact = FindObjectOfType<InteractionController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other) {
        if (other.GetComponent<PickupObject>()) {

            other.GetComponent<Rigidbody>().isKinematic = true;
            if (other.gameObject.Equals(interact.heldObject)) {

                interact.DropObject();
            }
            Activate();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<PickupObject>()) {
            Deactivate();
        }
    }
}

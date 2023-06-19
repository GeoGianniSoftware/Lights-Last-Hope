using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    public Text interactText;
    public float interactDistance;


    public PickupObject heldObject = null;
    public Transform heldObjectPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GrabObject(PickupObject objectToGrab) {
        DropObject();

        heldObject = objectToGrab;
        heldObject.transform.position = heldObjectPoint.position;
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
        heldObject.transform.SetParent(heldObjectPoint);
    }

    public void DropObject() {
        if (heldObject != null) {
            heldObject.transform.SetParent(null);
            heldObject.GetComponent<Rigidbody>().isKinematic = false;
            heldObject = null;
        }

    }

    // Update is called once per frame
    void Update()
    {


        if(heldObject != null) {
            interactText.text = "Press 'E' To Drop";
            if (Input.GetButtonDown("Interact")) {
                DropObject();
            }
        }
        else {
            Transform cam = Camera.main.transform;
            Ray ray = new Ray(cam.position, cam.forward);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, interactDistance)) {
                if (hit.transform.GetComponent<InteractText>()) {
                    interactText.text = hit.transform.GetComponent<InteractText>().text;
                    if (Input.GetButtonDown("Interact")) {
                        hit.transform.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
                    }

                }
                else {
                    interactText.text = "";
                }
            }
            else {
                interactText.text = "";
            }
        }

        
        
    }
}

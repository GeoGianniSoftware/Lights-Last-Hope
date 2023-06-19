using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractText))]
public class IOInput : MonoBehaviour
{
    public bool activated;
    public bool disabled;
    public bool permanent;
    public GameObject statusLight;
    public Material activeMat;
    public Material deactivatedMat;
    public Material disabledMat;

    InteractText Text;

    private void Awake() {
        Text = GetComponent<InteractText>();
        if(statusLight != null)
        statusLight.GetComponent<MeshRenderer>().material = deactivatedMat;
    }

    public void Activate() {
        activated = true;

        if(statusLight != null) {
            statusLight.GetComponent<MeshRenderer>().material = activeMat;
        }

        if (permanent)
            Disable();
    }

    public void Deactivate() {
        if (activated && permanent) {
            return;
        }
            
        activated = false;
        if (statusLight != null) {
            statusLight.GetComponent<MeshRenderer>().material = deactivatedMat;
        }
    }

    public void Disable() {
        disabled = true;
        statusLight.GetComponent<MeshRenderer>().material = disabledMat;
        Text.text = "Disabled";
    }

    public void Interact() {
        if (activated)
            Deactivate();
        else
            Activate();
    }
    
}

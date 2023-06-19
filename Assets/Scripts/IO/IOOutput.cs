using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractText))]
public class IOOutput : MonoBehaviour
{
    public enum OutputType
    {
        AND,
        OR
    }
    public OutputType TYPE;
    public bool activated;
    public List<IOInput> InputList;
    

    InteractText Text;

    private void Awake() {
        Text = GetComponent<InteractText>();
    }

    private void Update() {
        activated = getStatus();
    }

    public bool getStatus() {
        if (InputList.Count == 0)
            return true;

        switch (TYPE) {

            case OutputType.AND:
                bool temp = true;
                foreach (IOInput input in InputList)
                    if (input.activated == false)
                        temp = false;

                return temp;
               
            case OutputType.OR:
                bool temp1 = false;
                foreach (IOInput input in InputList)
                    if (input.activated == true)
                        temp1 = true;
                return temp1;
        }

        return false;
    }

    public void Activate() {
        activated = true;

        
    }

    public void Deactivate() {
       
            
        activated = false;
        
    }

    

    public void Toggle() {
        if (activated)
            Deactivate();
        else
            Activate();
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicDoor : IOOutput
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Animator>())
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(anim == null) {
            if (activated) {
                this.GetComponent<MeshRenderer>().enabled = false;
                this.GetComponent<Collider>().enabled = false;
                this.GetComponent<NavMeshObstacle>().enabled = false;
            }
            else {
                this.GetComponent<MeshRenderer>().enabled = true;
                this.GetComponent<Collider>().enabled = true;

                this.GetComponent<NavMeshObstacle>().enabled = true;
            }
        }
        else {
            if (activated) {

                print("TRUE");
                anim.SetBool("Activated", true);
            }
            else {
                print("FALSE");
                anim.SetBool("Activated", false);
            }
        }

        activated = getStatus();

       

    }

    
}

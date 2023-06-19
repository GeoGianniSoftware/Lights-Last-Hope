using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchScript : MonoBehaviour
{
     Rigidbody RB;
    public List<GameObject> lights = new List<GameObject>();

    public bool isBeingHeld = true;
    public float spotLightHeight;
    // Start is called before the first frame update
    void Start()
    {

        RB = GetComponent<Rigidbody>();
        RB.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        RB.isKinematic = isBeingHeld;

        if (lights[0] != null) {

            lights[0].transform.position = transform.position + (Vector3.up * spotLightHeight);
            lights[0].transform.LookAt(transform.position);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDistance : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    public float renderRange;
    void Start()
    {
        if(FindObjectOfType<ThidPersonMovement>())
        player = FindObjectOfType<ThidPersonMovement>().transform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null) {
            Vector3 dir = player.transform.position - transform.position;
            float length = dir.magnitude;

            if (player != null) {
                if (length > renderRange) {
                    if (transform.childCount == 0) {
                        this.GetComponent<Light>().enabled = false;
                    }
                    else {
                        transform.GetChild(0).gameObject.SetActive(false);
                        transform.GetChild(1).gameObject.SetActive(false);
                    }

                }
                else {
                    if (transform.childCount == 0) {
                        this.GetComponent<Light>().enabled = true;
                    }
                    else {

                        transform.GetChild(0).gameObject.SetActive(true);
                        transform.GetChild(1).gameObject.SetActive(true);
                    }
                }
            }
        }
       
        


    }
}

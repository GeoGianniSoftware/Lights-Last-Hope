using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepWater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Player") {
            Damage dmg = new Damage(1, true);
            dmg.setSourceAndPos(gameObject, transform.position);
            other.transform.SendMessage("TakeDamage", dmg, SendMessageOptions.DontRequireReceiver);
        }
    }
}

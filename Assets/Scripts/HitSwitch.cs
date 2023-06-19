using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSwitch : IOInput
{
    public Material hit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage() {
        GetComponent<MeshRenderer>().material = hit;
        Activate();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.GetComponent<EnemyWeapon>()) {
            print("DING!");
        }
    }
}

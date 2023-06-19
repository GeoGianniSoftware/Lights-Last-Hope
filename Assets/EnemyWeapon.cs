using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [System.NonSerialized]
    public Damage dmg;
    float hitDelay = 1f;
    float hitTimer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hitTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy")
            return;
        if(hitTimer <= 0) {


            dmg.setSourceAndPos(this.gameObject, transform.position);
            other.SendMessage("TakeDamage", dmg, SendMessageOptions.DontRequireReceiver);
            hitTimer = hitDelay;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        print("DING!");
    }
}

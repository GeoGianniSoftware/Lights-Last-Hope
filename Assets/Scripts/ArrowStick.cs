using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowStick : MonoBehaviour
{
    public Enemy attachedEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(attachedEnemy.currentHealth <= 0) {
            Destroy(gameObject);
        }
    }
}

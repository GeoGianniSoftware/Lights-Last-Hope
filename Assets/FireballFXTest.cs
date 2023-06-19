using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballFXTest : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform fireballPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Trigger() {
        Instantiate(fireballPrefab, fireballPos.position, fireballPos.rotation);
    }
}

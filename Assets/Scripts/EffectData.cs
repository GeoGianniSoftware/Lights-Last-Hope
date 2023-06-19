using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectData : MonoBehaviour
{
    public float lifeTime;
    public Quaternion effectRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetupEffect(float lifeT) {
        transform.rotation = effectRotation;
        lifeTime = lifeT;
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

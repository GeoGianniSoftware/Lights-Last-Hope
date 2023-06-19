using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticTrap : IOOutput
{
    public IAbility trapAbility;
    public float trapTime = 3f;
    public float timeOffset;
    public Vector2 maxOffset;
    float t = 0f;
    // Start is called before the first frame update
    void Start()
    {
        trapAbility = IAbility.Instantiate(trapAbility);
        trapAbility.CASTER = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        activated = getStatus();
        t -= Time.deltaTime * timeOffset;
        if(t <= 0 && activated) {
            Vector3 randomOffset = new Vector3(Random.Range(-maxOffset.x,maxOffset.x), Random.Range(-maxOffset.y, maxOffset.y), 0);
            trapAbility.setupProjectile(transform.forward, transform.position + transform.forward + randomOffset);
            trapAbility.Cast();
            t = trapTime;
        }

        
    }
}

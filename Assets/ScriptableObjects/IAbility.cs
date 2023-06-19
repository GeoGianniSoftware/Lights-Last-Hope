using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newAbility", menuName = "ScriptableObject/Create Ability", order = 0)]
public class IAbility : ScriptableObject
{
    public enum AbilityType
    {
        Projectile,
        Debuff,
        Buff,
        Ultimate
    }
    public AbilityType TYPE;
    public bool COMPLEX;
    public float COST;
    public float COOLDOWN;
    
    public GameObject CASTER;

    public Damage damage;

    [Header("Projectile")]
    public GameObject projectilePrefab;
    public float projectileStrength;
    public float gravityMod;
    public bool projectileSticks;
    public float lifeTime;

    [Header("Buffs & Debuffs")]
    public float speedMod = 0f;

    [Header("Effects")]
    public GameObject effectPrefab;
    public float effectLifetime;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 direction;
    public void setDirection(Vector3 dir) {
        direction = dir;
    }


    Vector3 targetPoint;
    public void setTargetPoint(Vector3 tp) {
        targetPoint = tp;
    }


    public void setupProjectile(Vector3 dir, Vector3 target) {
        setDirection(dir);
        setTargetPoint(target);
    }

    public void Cast() {
        CASTER.SendMessage("LoseEnergy", COST, SendMessageOptions.DontRequireReceiver);


        switch (TYPE) {
            case AbilityType.Projectile:
                Vector3 dir = direction;
                dir.y = 0;
                
                GameObject projectileSpawn = Instantiate(projectilePrefab, CASTER.transform.position + Vector3.up / 2.5f + dir, Quaternion.identity);
                projectileSpawn.transform.rotation = CASTER.transform.rotation;
                projectileSpawn.GetComponent<BasicProjectile>().damage = damage;
                projectileSpawn.GetComponent<BasicProjectile>().sticks = projectileSticks;
                projectileSpawn.GetComponent<BasicProjectile>().lifeTime = lifeTime;
                projectileSpawn.GetComponent<BasicProjectile>().gravityMod = gravityMod;


                if (projectileSpawn.GetComponent<BasicProjectile>())
                    projectileSpawn.GetComponent<BasicProjectile>().AddForce((targetPoint - CASTER.transform.position).normalized * projectileStrength, ForceMode.Impulse);
                break;
            case AbilityType.Debuff:
                BasicEnemy temp = null;
                Collider[] cols = Physics.OverlapSphere(targetPoint, 2f);
                float minDist = float.MaxValue;
                foreach(Collider c in cols) {
                    if(c.GetComponent<BasicEnemy>() && Vector3.Distance(targetPoint, c.transform.position) < minDist) {
                        minDist = Vector3.Distance(targetPoint, c.transform.position);
                        temp = c.GetComponent<BasicEnemy>();
                    }
                }
                if(temp != null) {

                    temp.SendMessage("DebuffSpeed", new Debuff(speedMod, effectLifetime));
                    SpawnEffect(temp.gameObject);
                }
                break;
        }
    }

    public void Cast(Transform pos) {
        CASTER.SendMessage("LoseEnergy", COST, SendMessageOptions.DontRequireReceiver);


        switch (TYPE) {
            case AbilityType.Projectile:
                Vector3 dir = direction;
                dir.y = 0;

                GameObject projectileSpawn = Instantiate(projectilePrefab, pos.transform.position, Quaternion.identity);
                projectileSpawn.transform.rotation = CASTER.transform.rotation;
                projectileSpawn.GetComponent<BasicProjectile>().damage = damage;
                projectileSpawn.GetComponent<BasicProjectile>().sticks = projectileSticks;
                projectileSpawn.GetComponent<BasicProjectile>().lifeTime = lifeTime;
                projectileSpawn.GetComponent<BasicProjectile>().gravityMod = gravityMod;


                if (projectileSpawn.GetComponent<BasicProjectile>())
                    projectileSpawn.GetComponent<BasicProjectile>().AddForce((targetPoint - CASTER.transform.position).normalized * projectileStrength, ForceMode.Impulse);
                break;
            case AbilityType.Debuff:
                BasicEnemy temp = null;
                Collider[] cols = Physics.OverlapSphere(targetPoint, 2f);
                float minDist = float.MaxValue;
                foreach (Collider c in cols) {
                    if (c.GetComponent<BasicEnemy>() && Vector3.Distance(targetPoint, c.transform.position) < minDist) {
                        minDist = Vector3.Distance(targetPoint, c.transform.position);
                        temp = c.GetComponent<BasicEnemy>();
                    }
                }
                if (temp != null) {

                    temp.SendMessage("DebuffSpeed", new Debuff(speedMod, effectLifetime));
                    SpawnEffect(temp.gameObject);
                }
                break;
        }
    }

    void SpawnEffect(GameObject target) {
        if (effectPrefab != null) {
            GameObject effect = Instantiate(effectPrefab, target.transform.position - (Vector3.up*target.transform.localScale.y), Quaternion.identity);
            effect.transform.SetParent(target.transform, false);
            effect.transform.position = target.transform.position - (Vector3.up * target.transform.localScale.y/2);
            if (effect.GetComponent<EffectData>())
                effect.GetComponent<EffectData>().SetupEffect(effectLifetime);
        }
    }

    public class Debuff {
        public float debuffAmount;
        public float debuffTime;

        public Debuff(float amt, float time) {
            debuffAmount = amt;
            debuffTime = time;
        }
    }

}

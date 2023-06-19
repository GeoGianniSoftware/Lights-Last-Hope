using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinController : Enemy
{
    

    public Transform target;
    NavMeshAgent NMA;
    Animator ANIM;
    [Header("Health")]


    [Header("Combat")]
    public float speed;
    public float agroRange = 5f;
    public float attackCooldown;

    [Header("Weapons")]

    float cooldownTimer;
    float resetTimer;
    bool attacking;

    public Collider kickCollider, spearCollider, shieldCollider;
    public Damage kickDamage, spearDamage, shieldDamage;

    public List<Collider> ragdoll = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        NMA = GetComponent<NavMeshAgent>();
        ANIM = GetComponent<Animator>();
        NMA.speed = speed;
        target = FindObjectOfType<ThidPersonMovement>().transform;

        kickCollider.GetComponent<EnemyWeapon>().dmg = kickDamage;

        spearCollider.GetComponent<EnemyWeapon>().dmg = spearDamage;

        shieldCollider.GetComponent<EnemyWeapon>().dmg = shieldDamage;
    }

    // Update is called once per frame
    void Update()
    {
        ANIM.SetFloat("Speed", NMA.velocity.magnitude);

        resetTimer -= Time.deltaTime;
        cooldownTimer -= Time.deltaTime;

        if(resetTimer <= 0) {

            int attackType = Random.Range(0, 3);
            ANIM.SetInteger("AttackType", attackType);
            resetTimer = 1f;
        }


        ANIM.SetBool("Attacking", attacking);
        float SqrDistance = float.MaxValue;
        if (target != null)
         SqrDistance = Vector3.SqrMagnitude(target.transform.position - transform.position);

        if ( SqrDistance <= agroRange*agroRange && !Aggroed) {
            ANIM.SetTrigger("Taunt");
            
            Aggroed = true;

        }
        else if(Aggroed && SqrDistance > (agroRange*agroRange)*2) {
            Aggroed = false;
        }


        if (target != null && Aggroed && NMA.isActiveAndEnabled) {
                NMA.destination = target.position;


            if(NMA.remainingDistance <= 3f && cooldownTimer <= 0) {
                if(NMA.remainingDistance <= 1.5f) {
                    Vector3 newLookPos = target.transform.position;
                    newLookPos.y = transform.position.y;
                    transform.LookAt(newLookPos);
                }
                attacking = true;
                cooldownTimer = attackCooldown;
            }
            else {
                attacking = false;
            }
        }
    }

    

    public void HaultMovement() {
        NMA.speed = 0;
    }


    public enum ColliderParam
    {
        Spear,
        Shield,
        Kick
    }
    public void EnableCollider(ColliderParam go) {
        Collider temp = null;
        switch (go) {
            case ColliderParam.Kick:
                temp = kickCollider;
                break;
            case ColliderParam.Shield:
                temp = shieldCollider;
                break;
            case ColliderParam.Spear:
                temp = spearCollider;
                break;
        }

        temp.enabled = true;
    }

    public void DisableCollider(ColliderParam go) {
        Collider temp = null;
        switch (go) {
            case ColliderParam.Kick:
                temp = kickCollider;
                break;
            case ColliderParam.Shield:
                temp = shieldCollider;
                break;
            case ColliderParam.Spear:
                temp = spearCollider;
                break;
        }

        temp.enabled = false;
    }

    public void ResumeMovement() {
        NMA.speed = speed;
    }

    public void ResetTaunt() {
        ANIM.ResetTrigger("Taunt");
    }

    public void TakeDamage(Damage dmg) {
        currentHealth -= dmg.amount;
        if (currentHealth <= 0)
            Die();
    }

    public void Die() {
        ANIM.SetTrigger("Death");
        ANIM.SetInteger("DeathType", Random.Range(0, 4));
        NMA.enabled = false;
    }

    public void PostDeath() {
        this.GetComponent<CharacterController>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        EnableRagdoll();
        if(!gameObject.GetComponent<Rigidbody>())
            this.gameObject.AddComponent<Rigidbody>();
        this.enabled = false;
    }

    void EnableRagdoll() {
        foreach (Collider c in ragdoll) {
            c.enabled = true;
            //c.gameObject.AddComponent<Rigidbody>();
        }
    }
}

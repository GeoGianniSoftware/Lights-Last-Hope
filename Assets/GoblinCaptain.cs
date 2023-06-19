using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinCaptain : Enemy
{

    [Header("Health")]

    public Transform target;
    NavMeshAgent NMA;
    Animator ANIM;


    [Header("Combat")]
    public float speed;
    public float attackRange;
    public float attackAngle;
    public float agroRange = 5f;
    public float attackCooldown;

    [Header("Weapons")]

    float cooldownTimer;
    float resetTimer;
    bool attacking;

    public Collider kickCollider, spearCollider, shieldCollider;
    public Damage kickDamage, spearDamage, shieldDamage;

    public List<Collider> ragdoll = new List<Collider>();

    [Header("Boss Data")]
    public int attackPhase = 0;
    public List<int> currentAttacks;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        NMA = GetComponent<NavMeshAgent>();
        ANIM = GetComponent<Animator>();
        NMA.speed = speed;

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
            int attackType = 1;
            if (attackPhase == 0 || attackPhase == 1) {

                attackType = Random.Range(1, 3);
            }
            else if (attackPhase == 2) {
                attackRange = 8;
                attackType = Random.Range(0, 2);
            }
             
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


            if(NMA.remainingDistance <= attackRange + NMA.radius) {

                float targetAngle = (Vector3.Angle(target.position - transform.position, transform.forward));
                bool attackAngleReach = (targetAngle <= attackAngle);
                
                    
                Vector3 targetSpeed = target.GetComponent<ThidPersonMovement>().currentMoveVelocity;
                if (NMA.remainingDistance <= 1.5f) {
                    
                    Vector3 newLookPos = target.transform.position;
                    newLookPos.y = transform.position.y;
                    transform.LookAt(newLookPos);
                    
                }
                NMA.speed = 0;
                if(cooldownTimer <= 0 && attackAngleReach) {

                    attacking = true;
                    cooldownTimer = attackCooldown;
                }
            }
            else {
                if(canMove)
                    NMA.speed = speed;

                attacking = false;
            }
        }
    }


    public bool canMove = true;
    public void HaultMovement() {
        canMove = false;
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
        canMove = true;
        NMA.speed = speed;
    }

    public void ResetTaunt() {
        ANIM.ResetTrigger("Taunt");
    }

    public List<float> AttackPhases = new List<float>();
    public void TakeDamage(Damage dmg) {
        int deleteIndex = -1;

        currentHealth -= dmg.amount;
        if((currentHealth == (int)maxHealth * .25f||currentHealth == (int)maxHealth * .5f|| currentHealth == (int)maxHealth * .75f)) {
            for (int i = 0; i < AttackPhases.Count; i++) {
                if (currentHealth <= maxHealth * AttackPhases[i]) {
                    attackPhase++;
                    deleteIndex = i;
                }
            }
            if (deleteIndex != -1)
                AttackPhases.RemoveAt(deleteIndex);
        }
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
